using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [HideInInspector] public Rigidbody body;
    
    private float moveX;
    private float moveY;

    [SerializeField] private float speed = 0;
    void Start()
        => body = GetComponent<Rigidbody>();

    void OnMove(InputValue input)
    {
        var movement = input.Get<Vector2>();
        
        moveX = movement.x;
        moveY = movement.y;
    }

    private void FixedUpdate()
        => body.AddForce(new Vector3(moveX, 0, moveY) * speed);
}
