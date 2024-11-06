using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwarmController : MonoBehaviour
{
    [Serializable]
    public struct BoidOptions
    {
        public float Coherence;
        public float Separation;
        public float SeparationRange;
        public float Alignment;
        public float Targetting;
        public float TopSpeed;
        public float MinSpeed;
    }

    [Serializable]
    public struct SpawnOptions
    {
        public int InitialCount;
        public int MaxCount;
        public float SwarmRegenRate;
        public float SpawnRadius;
    }

    [SerializeField]
    private FloatingEnemyController _enemyPrefab;

    [SerializeField]
    private BoidOptions _boidOptions;

    [SerializeField]
    private SpawnOptions _spawnOptions;

    [SerializeField]
    private Transform _playerTarget;

    private List<FloatingEnemyController> _enemies = new();

    private Vector2 playerPos;

    private float timer;

    private void Awake()
    {
        for (var i = 0; i < _spawnOptions.InitialCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = _spawnOptions.SwarmRegenRate * _enemies.Count / _spawnOptions.MaxCount;
            if (_enemies.Count < _spawnOptions.MaxCount)
            {
                SpawnEnemy();
            }
        }

        // Remove dead enemies
        _enemies = _enemies.Where(a => a != null).ToList();

        Vector2 averagePos = Vector2.zero;
        Vector2 averageVel = Vector2.zero;
        foreach (FloatingEnemyController enemy in _enemies)
        {
            averagePos += (Vector2)enemy.transform.position;
            averageVel += enemy.Velocity;
        }

        averagePos /= _enemies.Count;
        averageVel /= _enemies.Count;

        float timeStep = Time.deltaTime;

        // Boid behavior
        foreach (FloatingEnemyController enemy in _enemies)
        {
            Vector2 currentPos = enemy.transform.position;
            if (_playerTarget != null)
            {
                playerPos = _playerTarget.position;
            }

            Vector2 desiredTargetVel = (playerPos - currentPos).normalized * _boidOptions.TopSpeed;
            Vector2 desiredCohesion = averagePos - currentPos;
            Vector2 desiredAlignment = averageVel;

            float distanceToPlayer = (playerPos - currentPos).magnitude;

            enemy.Velocity = Vector2.MoveTowards(enemy.Velocity,
                desiredTargetVel,
                _boidOptions.Targetting * timeStep * distanceToPlayer * distanceToPlayer);

            enemy.Velocity = Vector2.MoveTowards(enemy.Velocity, desiredCohesion,
                _boidOptions.Coherence * timeStep);

            enemy.Velocity = Vector2.MoveTowards(enemy.Velocity, desiredAlignment,
                _boidOptions.Alignment * timeStep);

            Vector2 separationVector = Vector2.zero;
            foreach (FloatingEnemyController otherEnemies in _enemies)
            {
                if (otherEnemies == enemy)
                {
                    continue;
                }

                Vector2 otherPos = otherEnemies.transform.position;
                Vector2 toOther = otherPos - currentPos;
                float distance = toOther.magnitude;

                if (distance < _boidOptions.SeparationRange)
                {
                    separationVector -= toOther;
                }
            }

            enemy.Velocity += separationVector * (_boidOptions.Separation * timeStep);

            enemy.Velocity = Vector2.ClampMagnitude(enemy.Velocity, _boidOptions.TopSpeed);
            if (enemy.Velocity.magnitude < _boidOptions.MinSpeed)
            {
                enemy.Velocity = enemy.Velocity.normalized * _boidOptions.MinSpeed;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _spawnOptions.SpawnRadius);
    }

    private void SpawnEnemy()
    {
        FloatingEnemyController enemy = Instantiate(_enemyPrefab,
            transform.position + (Vector3)Random.insideUnitCircle.normalized * _spawnOptions.SpawnRadius,
            Quaternion.identity);
        enemy.transform.SetParent(transform);
        _enemies.Add(enemy);
    }
}