using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ShootyScript : MonoBehaviour
{
    [Serializable]
    public struct GunStats
    {
        public float FireDelay;
        public float ProjectileSpeed;
        public float ProjectileSpread;
        public float ProjectileLifetime;
    }

    [SerializeField]
    private ProjectileScript _projectilePrefab;

    [SerializeField]
    private SpriteRenderer _weaponSprite;

    [SerializeField]
    private Transform _weaponPivot;

    [SerializeField]
    private Transform _projectileSpawnPoint;

    [SerializeField]
    private GunStats _weaponStats;

    public UnityEvent OnFire;

    private float _fireTimer;

    private void Update()
    {
        _fireTimer -= Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            Vector2 aimDir = GetMouseDir();
            RotateWeaponSprite(aimDir);

            if (_fireTimer < 0f)
            {
                FireProjectile(aimDir);
                _fireTimer = _weaponStats.FireDelay;
                OnFire?.Invoke();
            }

            _weaponSprite.enabled = true;
        }
        else
        {
            _weaponSprite.enabled = false;
        }
    }

    private void FireProjectile(Vector2 dir)
    {
        // Fire bullet
        ProjectileScript projectile =
            Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);

        float spreadAngle = Random.Range(-_weaponStats.ProjectileSpread / 2, _weaponStats.ProjectileSpread / 2);
        dir = Quaternion.Euler(0, 0, spreadAngle) * dir;
        projectile.Fire(dir, _weaponStats.ProjectileSpeed, dir.x < 0, _weaponStats.ProjectileLifetime);
    }

    private void RotateWeaponSprite(Vector2 dir)
    {
        Quaternion aimRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
        _weaponPivot.rotation = aimRotation;

        _weaponSprite.flipY = dir.x < 0;
    }

    private Vector2 GetMouseDir()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gunPos = _weaponPivot.position;

        Vector2 vector = mousePosition - gunPos;
        return vector.normalized;
    }
}