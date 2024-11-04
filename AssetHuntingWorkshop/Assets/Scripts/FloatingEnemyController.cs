using UnityEngine;
using UnityEngine.UIElements;

public class FloatingEnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 10;
    [HideInInspector] public int health;

    private PlayerMovementController player;
    private Rigidbody2D enemyRigidbody;
    private bool alerted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        player = FindFirstObjectByType<PlayerMovementController>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        if (player != null)
        {
            alerted = true; // Could add an aggro system later.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alerted) {
            Vector3 targetLoc = player.gameObject.transform.position;
            Vector3 pursueDirection = targetLoc - transform.position;
            Vector2 pursueDirection2DN = new Vector2(pursueDirection.x, pursueDirection.y).normalized;
            
            enemyRigidbody.linearVelocity = pursueDirection2DN * moveSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health -= 1;
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
