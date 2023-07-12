using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class ShooterHealthController : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private Material deathMaterial;
        [SerializeField] private new SkinnedMeshRenderer renderer;
        
        private ShooterController _shooterController;
        private CapsuleCollider _collider;
        private Animator _animator;
        private Rigidbody _rb;
        private bool _isDead;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            _shooterController = GetComponent<ShooterController>();
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
            _animator=transform.GetChild(_shooterController.GetSoldierLevel()).GetComponent<Animator>();
            renderer = transform.GetChild(_shooterController.GetSoldierLevel()).GetChild(1)
                .GetComponent<SkinnedMeshRenderer>();
            _animator.SetTrigger("isDead");
            _isDead = true;
            _collider.isTrigger = false;
            renderer.material.DOColor(deathMaterial.color, 1);
            _shooterController.Die();
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _rb.AddExplosionForce(takenDamage+5, impactPosition, 6, .01f, ForceMode.VelocityChange);
            StartCoroutine(DeathDelay());
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }

        public void IncreaseHealth()
        {
            health += 10;
        }
    }
}