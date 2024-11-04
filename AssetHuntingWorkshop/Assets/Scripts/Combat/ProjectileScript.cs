using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _bulletSprite;

    [SerializeField]
    private Rigidbody2D _rb;

    private float _lifeTimer;

    private void Update()
    {
        RotateTowardsVelocity();
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void RotateTowardsVelocity()
    {
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, _rb.linearVelocity));
    }

    public void Fire(Vector2 dir, float speed, bool flip, float lifeTime)
    {
        _bulletSprite.flipY = flip;
        _rb.linearVelocity = dir.normalized * speed;
        _lifeTimer = lifeTime;
        RotateTowardsVelocity();
    }
}