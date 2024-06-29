using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    #region Variables
    
    [SerializeField] public float speed = 500;
    [SerializeField] private float acceleration = 1.1f;

    [SerializeField] private Direction ballDirection;
    [SerializeField] private Vector2 initialPos;
    [SerializeField] private GameObject score;
    
    [SerializeField] private Color[] colors;
    
    [SerializeField] private AudioClip ballBounce;
    [SerializeField] private AudioClip ballPowerUp;

    [SerializeField] private CameraShake cameraShake;

    private AudioSource audio;
    
    private Renderer sphereRenderer;
    
    private Vector3 velocity;

    [HideInInspector] public Rigidbody rb;
    
    public static bool Play = true;
    
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    #endregion

    #region Init
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        sphereRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        
        score.GetComponent<ScoreHandler>().UpdateScore();
        ballDirection = Random.Range(0f, 1f) < 0.5 ? Direction.Left : Direction.Right;
        
        ResetBallPosition();
        StartMovement();
    }

    private void Update()
        => velocity = rb.velocity;

    #endregion

    #region Methods
    private void ResetBallPosition()
    {
        velocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = initialPos;
    }

    private void StartMovement()
        => rb.AddForce(new Vector2(1, ballDirection == Direction.Left ? -1f : 1f) * speed);

    private void OnPaddleCollision(float pitch)
    {
        rb.velocity *= acceleration;
        audio.pitch += pitch;
        audio.Play();
    }

    private IEnumerator OnPlayerLose()
    {
        ResetBallPosition();
        cameraShake.ChangeBackground();
        yield return new WaitForSeconds(2);
        
        // if game is not over
        if (Play) StartMovement();
        else score.GetComponent<ScoreHandler>().UpdateScore();
    }

    private void ChangeColor()
    {
        Color newColor = colors[Random.Range(0, colors.Length)];
        Color color = sphereRenderer.material.color;
        
        while (newColor == color)
            newColor = colors[Random.Range(0, colors.Length)];

        sphereRenderer.material.SetColor(Color1, newColor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ChangeColor();
        StartCoroutine(cameraShake.Shake(0.05f, 0.3f));
        audio.clip = ballBounce;
        audio.pitch = 1;
        
        if (collision.gameObject.CompareTag("Lose"))
        {
            Player player = collision.gameObject.GetComponent<PlayerScore>().player;
            score.GetComponent<ScoreHandler>().UpdateScore(player);

            ballDirection = player == Player.Player1 ? Direction.Left : Direction.Right;
            StartCoroutine(OnPlayerLose());
            return;
        }

        var velocityMagnitude = velocity.magnitude;
        // reflect function gets the normal angle of the collision
        Vector3 direction = Vector3.Reflect(velocity.normalized, collision.contacts[0].normal);
        Vector3 newVelocity = direction * Mathf.Max(velocityMagnitude, 0f);
        
        rb.velocity = newVelocity.y switch
        {
            < 0 and > -10 => new Vector2(-10, -10),
            > 0 and < 10 => new Vector2(10, 10),
            _ => newVelocity
        };

        // increase speed if touch a paddle
        if (collision.gameObject.CompareTag("Paddle"))
        {
            var pitch = Math.Abs(collision.contacts[0].otherCollider.bounds.center.x - collision.contacts[0].point.x) 
                        / collision.contacts[0].otherCollider.bounds.extents.x;
            
            OnPaddleCollision(pitch);
        }

        else
        {
            // PowerUp random change speed
            if (Random.Range(0f, 1f) < 0.1f)
            {
                audio.clip = ballPowerUp;
                rb.velocity *= Random.Range(0.8f, 1.2f) * (Random.Range(0f, 1f) < 0.5f ? -1 : 1);
            }
            
            audio.Play();
        }
    }

    #endregion
}
