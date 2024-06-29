using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float acceleration = 0.2f;
    [SerializeField] private float maxSpeed = 3f;

    private float coyoteTimeVoidJump = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferVoidJump = 0.01f;
    private float jumpBufferCounter;
    
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float jumpBoost = 0.5f;

    private int score = 0;

    private Animator animator;
    
    private RaycastHit hit;
    
    private Vector3 camOffset;
    private Vector3 camPos;

    private Rigidbody rb;
    private Collider collider;
    private Camera camera;

    public bool isGrounded;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    private void Awake()
    {
        camera = Camera.main;
        camOffset = camera.transform.position - transform.position;
        camPos = camera.transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        animator.SetFloat(Speed, rb.velocity.magnitude);
        animator.SetBool(Jumping, !isGrounded);
        
        if (Timer.GameOver)
        {
            if (rb.velocity.x > 0)
            {
                transform.position += new Vector3(0.5f, 0, 0);
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (!isGrounded)
            {
                transform.position -= new Vector3(0, 0.1f, 0);
                isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
            }
            return;
        }

        // sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
            maxSpeed = 8;
        // walk
        if (Input.GetKeyUp(KeyCode.LeftShift))
            maxSpeed = 5;
        
        var movement = Input.GetAxis("Horizontal");

        // change mario looking position smoothly
        var direction = Quaternion.LookRotation(new Vector3(movement < 0 ? -1 : 1, 0, 0));
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, 0.1f);

        // speed x axis
        rb.velocity += movement * Vector3.right * Time.deltaTime * acceleration;

        // check if mario touches the ground
        float halfHeight = collider.bounds.extents.y + 0.03f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, halfHeight);

        // clamp the velocity of mario
        Vector3 velocity = rb.velocity;
        velocity = new Vector3(Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed), velocity.y, velocity.z);
        rb.velocity = velocity;

        // coyote
        coyoteTimeCounter = isGrounded ? coyoteTimeVoidJump : coyoteTimeCounter - Time.deltaTime;
        
        // jump buffering
        jumpBufferCounter = Input.GetButtonDown("Jump") ? jumpBufferVoidJump : jumpBufferCounter - Time.deltaTime;
        
        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            Jump();
            jumpBufferCounter = 0;
        }

        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            JumpBoost();
            coyoteTimeCounter = 0;
        }

        // camera follow the player
        var position = transform.position;
        camera.transform.position = new Vector3(position.x + camOffset.x, camPos.y, camPos.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Physics.Raycast(transform.position + 2.2f*Vector3.up, Vector3.up, out RaycastHit objectHit, 2))
        {
            if (collision.transform.CompareTag("Brick") && objectHit.collider.transform.CompareTag("Brick"))
                ScoreMoneyHandler.Instance.OnBrickHitted(collision.gameObject);

            else if (collision.transform.CompareTag("Mystery") && objectHit.collider.transform.CompareTag("Mystery"))
                ScoreMoneyHandler.Instance.OnMysteryBlockHitted(collision.gameObject);
        }
    }


    private void Jump()
        => rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    private void JumpBoost()
        => rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpBoost);
}
