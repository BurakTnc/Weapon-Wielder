using System;
using System.Collections;
using _Root.Scripts.Enums;
using _Root.Scripts.Managers;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class EnemyHealthController : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private Material deathMaterial;
        [SerializeField] private new SkinnedMeshRenderer renderer;
        
        private EnemyShooterController _enemyShooter;
        private EnemyMovementController _enemyMovement;
        private CapsuleCollider _collider;
        private Rigidbody _rb;
        private Animator _animator;
        private float _startHealth;
        private bool _isDead;

        private void Awake()
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider>();
            _enemyShooter = GetComponent<EnemyShooterController>();
            _enemyMovement = GetComponent<EnemyMovementController>();
        }

        private void Start()
        {
            _startHealth = health;
        }

        public void GetHit(float takenDamage,Vector3 impactPosition)
        {
            health -= takenDamage;
            if (health <= 0)
            {
                Die(impactPosition,takenDamage);
            }
        }

        private void Die(Vector3 impactPosition,float takenDamage)
        { 
            if(_isDead)
                return;
            LevelSignals.Instance.OnEnemyKilled?.Invoke();
            _animator.SetTrigger("isDied");
            _isDead = true;
            _collider.isTrigger = false;
            renderer.material.DOColor(deathMaterial.color, 1);
            _enemyMovement.Die();
            _enemyShooter.Die();
            var particle = PoolManager.Instance.GetPooledObject(PooledObjectType.MoneyParticle);
            particle.transform.position = transform.position;
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.AddExplosionForce(takenDamage, impactPosition, 5, .1f, ForceMode.VelocityChange);
            StartCoroutine(DeathDelay());
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }

        public void ResetSoldier()
        {
            Destroy(_rb);
            health = _startHealth;
            _enemyShooter.ResetSoldier();
            _enemyMovement.ResetSoldier();
        }
    }
}