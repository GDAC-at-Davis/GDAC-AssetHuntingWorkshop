using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{
    private const float JUMP_LEEWAY_Y_RANGE = 0.2f;
    private const float JUMP_LEEWAY_X_MOD = 1.25f;

    [SerializeField]
    private float moveSpeed = 7.5f;

    [SerializeField]
    private float jumpForce = 400;

    [SerializeField]
    private float heldJumpGravityMult = 0.7f; // Gravity is this times as strong if you are holding the jump button

    [SerializeField]
    private float shootingMoveSpeed;

    // Some code reused from Gnome Globe. Credits to Thomas Watson
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    public UnityEvent OnJumpEvent;

    [SerializeField]
    private SpriteRenderer _sprite;

    private float defaultGravity;

    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        defaultGravity = playerRigidbody.gravityScale;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            playerRigidbody.linearVelocityY = 0;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            OnJumpEvent?.Invoke();
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

        if (movement < 0)
        {
            _sprite.flipX = true;
        }
        else if (movement > 0)
        {
            _sprite.flipX = false;
        }

        bool shooting = Input.GetMouseButton(0);

        playerRigidbody.linearVelocityX = movement * (shooting ? shootingMoveSpeed : moveSpeed);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position,
            new Vector2(transform.localScale.x * JUMP_LEEWAY_X_MOD, JUMP_LEEWAY_Y_RANGE), 0f, groundLayer);
    }
}