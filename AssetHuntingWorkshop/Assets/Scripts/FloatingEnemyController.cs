using UnityEngine;

public class FloatingEnemyController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private int maxHealth = 10;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [HideInInspector]
    public int health;

    public Vector2 Velocity => _rb.linearVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (_rb.linearVelocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_rb.linearVelocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }

        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, _rb.linearVelocity));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }

    public void SteerTowards(Vector2 desiredVelocity, float amount)
    {
        Vector2 vel = _rb.linearVelocity;
        Vector2 newVel = Vector2.MoveTowards(vel, desiredVelocity, amount);
        _rb.linearVelocity = newVel;
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}