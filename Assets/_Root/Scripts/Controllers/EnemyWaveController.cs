using System;
using _Root.Scripts.Enums;
using _Root.Scripts.Managers;
using _Root.Scripts.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Controllers
{
    public class EnemyWaveController : MonoBehaviour
    {
        [SerializeField] private float spawnCoolDown;
        [SerializeField] private float xSpawnClamp;
        [SerializeField] private float zSpawnPos;
        [SerializeField] private int enemyCount;

        private int _killedEnemy;
        private int _spawnCount;
        private bool _isSpawning;
        private float _timer;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            LevelSignals.Instance.OnFight += Fight;
            LevelSignals.Instance.OnEnemyKilled += OnEnemyKilled;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnFight -= Fight;
            LevelSignals.Instance.OnEnemyKilled -= OnEnemyKilled;
        }

        private void Update()
        {
            BeginSpawn();
        }

        private void Fight()
        {
            _isSpawning = true;
        }

        private void OnEnemyKilled()
        {
            _killedEnemy++;
            if (_killedEnemy >= enemyCount) 
            {
                CoreGameSignals.Instance.OnLevelComplete?.Invoke();
            }
        }
        private void BeginSpawn()
        {
            if(!_isSpawning)
                return;

            if (_timer <= 0)
            {
                _timer += spawnCoolDown;
                Spawn();
            }

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, spawnCoolDown);

        }

        private void Spawn()
        {
            if(_spawnCount >=enemyCount)
                return;
            
            var soldier = PoolManager.Instance.GetPooledObject(PooledObjectType.Enemy);
            var chosenXPos = Random.Range(-xSpawnClamp, xSpawnClamp + .1f);
            var desiredPos = new Vector3(chosenXPos, -.5f, zSpawnPos);

            _spawnCount++;
            soldier.transform.position = desiredPos;

        }
    }
}