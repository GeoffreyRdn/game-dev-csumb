using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    #region Events

    public delegate void PlayerLose();
    public static event PlayerLose OnPlayerLose;

    public delegate void PlayerHit(int lives);
    public static event PlayerHit UpdateLife;
    
    private void OnEnable()
    {
        Enemy.OnWin += OnWin;
        GameHandler.Restart += Restart;
    }

    private void OnDisable()
    {
        Enemy.OnWin -= OnWin;
        GameHandler.Restart -= Restart;
    }

    #endregion

    #region Variables

    [FormerlySerializedAs("bullet")] public GameObject bulletPrefab;

    [FormerlySerializedAs("shootingOffset")]
    public Transform shootOffsetTransform;

    [SerializeField] private float speed;
    [SerializeField] private int lives = 3;
    [SerializeField] private int maxLives = 8;
    
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip shootSound;

    private Vector3 initialPosition;
    private Animator playerAnimator;
    private Rigidbody2D rigidbody;
    private GameObject bullet;

    private AudioSource audioSource;

    public bool canMove = true;

    private float hitAnimationDuration = 1f;
    private float currentHit = 0;
    
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Restart1 = Animator.StringToHash("Restart");
    private static readonly int Shoot1 = Animator.StringToHash("Shoot");

    #endregion

    #region Init
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        UpdateLife?.Invoke(lives);
    }

    private void Update()
    {
        if (GameHandler.hasLoose)
        {
            rigidbody.velocity = Vector2.zero;
            return;
        }

        if (!canMove)
        {
            // animation is running
            if (currentHit > 0) currentHit -= Time.deltaTime;

            // animation is done and player wants to move again
            else if (Input.GetAxis("Horizontal") != 0) canMove = true;
            return;
        }
        Vector3 position = transform.position;
        if (position.x is < -8 or > 8)
        {
            position.x = position.x < 0 ? -8 : 8;
            rigidbody.velocity = Vector2.zero;
            transform.position = position;
        }
        else Move();
        
        if (Input.GetKeyDown(KeyCode.Space) && !bullet) 
            Shoot();
    }
    
    #endregion
    
    #region Methods

    private void OnTriggerEnter2D(Collider2D bullet)
    {
        // player bullet
        if (bullet.transform.CompareTag("Enemy")) return;

        lives--;
        
        audioSource.clip = deathSound;
        audioSource.Play();
        
        GetComponentInChildren<ParticleSystem>().Play();
        
        Destroy(bullet.gameObject);
        if (lives == 0) PlayerDeath();
        else PlayerGotHit();
    }

    private IEnumerator ResetPlayerPosition()
    {
        yield return new WaitForSeconds(hitAnimationDuration-0.2f);
        Spawner.AnimationIsRunning = false;
        playerAnimator.SetBool(Hit, false);
        transform.position = initialPosition;
    }

    private void PlayerGotHit()
    {
        UpdateLife?.Invoke(lives);
           
        rigidbody.velocity = Vector2.zero;

        playerAnimator.SetBool(Hit, true);
        Spawner.AnimationIsRunning = true;
        canMove = false;
        currentHit = hitAnimationDuration;
        
        StartCoroutine(ResetPlayerPosition());
    }
    
    private void PlayerDeath()
    {
        GameHandler.hasLoose = true;
        
        OnPlayerLose?.Invoke();
        UpdateLife?.Invoke(lives);

        rigidbody.velocity = Vector2.zero;
        playerAnimator.SetTrigger(Death);
        StartCoroutine(LoadCredits());
    }

    IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene("Credits");
    }

    public static void OnLose()
    {
        OnPlayerLose?.Invoke();
        GameHandler.hasLoose = true;
    }
    
    private void OnWin()
    {
        if (lives == maxLives) return;
        
        lives++;
        UpdateLife?.Invoke(lives);
    }

    private void Restart()  
    {
        transform.position = initialPosition;
        lives = 3;
        UpdateLife?.Invoke(lives);
        canMove = true;
        playerAnimator.SetTrigger(Restart1);
    }
    
    #endregion

    #region Actions

    private IEnumerator PlayShootAnimation()
    {
        playerAnimator.SetBool(Shoot1, true);
        yield return new WaitForSeconds(0.3f);
        playerAnimator.SetBool(Shoot1, false);
    }
    
    private void Shoot()
    {
        audioSource.clip = shootSound;
        audioSource.Play();

        StartCoroutine(PlayShootAnimation());

        bullet = Instantiate(bulletPrefab, shootOffsetTransform.position, Quaternion.identity);
        Destroy(bullet, 1.7f);
    }

    private void Move()
        => rigidbody.velocity = Vector2.right * speed * Input.GetAxis("Horizontal");

    #endregion
}
