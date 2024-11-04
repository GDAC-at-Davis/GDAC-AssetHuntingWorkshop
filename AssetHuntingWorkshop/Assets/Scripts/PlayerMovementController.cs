using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    const float JUMP_LEEWAY_RANGE = 0.2f;

    [SerializeField] float moveSpeed = 7.5f;
    [SerializeField] float jumpForce = 400;
    [SerializeField] float heldJumpGravityMult = 0.7f; // Gravity is this times as strong if you are holding the jump button

    // Some code reused from Gnome Globe. Credits to Thomas Watson
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private float defaultGravity;



    private Rigidbody2D playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        defaultGravity = playerRigidbody.gravityScale;
    }

    void Update()
    {

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            playerRigidbody.linearVelocityY = 0;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButton("Jump") && !IsGrounded())
        {
            playerRigidbody.gravityScale = defaultGravity * heldJumpGravityMult;
        }
        else
        {
            playerRigidbody.gravityScale = defaultGravity;
        }

        float movement = Input.GetAxis("Horizontal");

        playerRigidbody.linearVelocityX = movement * moveSpeed;
    }
    
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, JUMP_LEEWAY_RANGE, groundLayer);
    }


}
