using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmController : MonoBehaviour
{
    [Serializable]
    public struct BoidOptions
    {
        public float Coherence;
        public float Separation;
        public float Alignment;
        public float Targetting;
        public float TopSpeed;
    }

    [Serializable]
    public struct SpawnOptions
    {
        public int InitialCount;
    }

    [SerializeField]
    private FloatingEnemyController _enemyPrefab;

    [SerializeField]
    private BoidOptions _boidOptions;

    [SerializeField]
    private SpawnOptions _spawnOptions;

    [SerializeField]
    private Transform _playerTarget;

    private List<FloatingEnemyController> _enemies;

    private void Awake()
    {
        for (var i = 0; i < _spawnOptions.InitialCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
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
            Vector2 playerPos = _playerTarget.position;

            Vector2 desiredTargetVel = (playerPos - currentPos).normalized * _boidOptions.TopSpeed;
            Vector2 desiredCohesion = (averagePos - currentPos).normalized * _boidOptions.TopSpeed;
            Vector2 desiredAlignment = averageVel.normalized * _boidOptions.TopSpeed;

            enemy.SteerTowards(desiredTargetVel, _boidOptions.Targetting * timeStep);
            enemy.SteerTowards(desiredCohesion, _boidOptions.Coherence * timeStep);
            enemy.SteerTowards(desiredAlignment, _boidOptions.Alignment * timeStep);

            foreach (FloatingEnemyController otherEnemies in _enemies)
            {
                if (otherEnemies == enemy)
                {
                    continue;
                }

                Vector2 otherPos = otherEnemies.transform.position;
                Vector2 toOther = otherPos - currentPos;
                float distance = toOther.magnitude;
                distance = Mathf.Max(0.1f, distance);

                enemy.SteerTowards(-toOther.normalized * _boidOptions.TopSpeed,
                    _boidOptions.Separation / distance * timeStep);
            }
        }
    }

    private void SpawnEnemy()
    {
        FloatingEnemyController enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        enemy.transform.SetParent(transform);
        _enemies.Add(enemy);
    }
}