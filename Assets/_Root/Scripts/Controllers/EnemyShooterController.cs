using System;
using _Root.Scripts.Enums;
using _Root.Scripts.Managers;
using _Root.Scripts.Objects;
using _Root.Scripts.Signals;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class EnemyShooterController : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float range;
        [SerializeField] private float fireRate;
        [SerializeField] private Transform shootingPos;

        private bool _isDead;
        private float _timer;


        private void OnEnable()
        {
            CoreGameSignals.Instance.OnLevelComplete += Stop;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnLevelComplete -= Stop;
        }

        private void Update()
        {
            BeginFire();
        }

        private void BeginFire()
        {
            if(_isDead)
                return;
            
            if (_timer <= 0)
            {
                _timer += fireRate;
                Fire();
            }

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, fireRate);

        }

        private void Fire()
        {
            var bullet = PoolManager.Instance.GetPooledObject(PooledObjectType.EnemyBullet);
            var fireEffect = PoolManager.Instance.GetPooledObject(PooledObjectType.Explosion);
            bullet.transform.position = shootingPos.position;
                
            if (bullet.gameObject.TryGetComponent(out Bullet firedBullet))
            {
                var fightingDirection = transform.forward * (40f);
                firedBullet.Fire(fightingDirection, range, damage, fireRate);
                fireEffect.transform.position = shootingPos.position;
            }
        }

        private void Stop()
        {
            _isDead = true;
        }

        public void Die()
        {
            _isDead = true;
        }

        public void ResetSoldier()
        {
            _isDead = false;
            _timer = 0;
        }
        
    }
}