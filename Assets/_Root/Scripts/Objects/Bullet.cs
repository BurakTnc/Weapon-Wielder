using System;
using System.Collections;
using _Root.Scripts.Controllers;
using _Root.Scripts.Enums;
using _Root.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace _Root.Scripts.Objects
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private bool isEnemy;
        
        private float _damage;
        private float _fireRate;
        private Vector3 _speed;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            //transform.position += _speed;
        }

        public void Fire(Vector3 speed,float range,float damage,float fireRate)
        {
            _speed = speed;
            _damage = damage;
            _fireRate = fireRate;
            //Debug.Log("FR: " + _fireRate + " DMG: " + _damage + " RNF: " + range);
            _rb.velocity = _speed;
           
            StartCoroutine(KillTheBullet(range));
        }

        private IEnumerator KillTheBullet(float range)
        {
            yield return new WaitForSeconds(range);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TargetDummyController dummy))
            {
                dummy.GetHit(_damage,transform.position,_fireRate);
                var particle = PoolManager.Instance.GetPooledObject(PooledObjectType.HitParticle);
                particle.transform.position = transform.position;
                HapticManager.Instance.PlayRigidHaptic();
                gameObject.SetActive(false);
            }

            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                gate.IncreaseGateStats();
                var particle = PoolManager.Instance.GetPooledObject(PooledObjectType.HitParticle);
                particle.transform.position = transform.position;
                HapticManager.Instance.PlaySelectionHaptic();
                gameObject.SetActive(false);
            }
            if (other.gameObject.TryGetComponent(out BonusSoldierPlatform bonusDummy))
            {
                bonusDummy.GetHit(_fireRate);
                var particle = PoolManager.Instance.GetPooledObject(PooledObjectType.HitParticle);
                particle.transform.position = transform.position;
                HapticManager.Instance.PlayLightHaptic();
                gameObject.SetActive(false);
            }

            if (other.gameObject.TryGetComponent(out EnemyHealthController enemy))
            {
                if(isEnemy)
                    return;
                enemy.GetHit(_damage,transform.position);
                HapticManager.Instance.PlaySelectionHaptic();
                gameObject.SetActive(false);
            }
            if (other.gameObject.TryGetComponent(out ShooterHealthController shooter))
            {
                if(!isEnemy)
                    return;
                
                shooter.GetHit(_damage,transform.position);
                gameObject.SetActive(false);
            }
        }
    }
}