using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private AudioClip _deathSound;

    public Vector2 Velocity
    {
        get => _rb.linearVelocity;
        set => _rb.linearVelocity = value;
    }

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
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(_deathSound, transform.position);
            Destroy(gameObject);
        }
    }
}