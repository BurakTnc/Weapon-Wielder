using System;
using System.Collections;
using _Root.Scripts.Controllers;
using Unity.VisualScripting;
using UnityEngine;

namespace _Root.Scripts.Objects
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _damage;
        private float _fireRate;
        private Vector3 _speed;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            transform.position += _speed;
        }

        public void Fire(Vector3 speed,float range,float damage,float fireRate)
        {
            _speed = speed;
            _damage = damage;
            _fireRate = fireRate;
            //_rb.velocity = speed;
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
                gameObject.SetActive(false);
            }

            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                gate.IncreaseGateStats();
                gameObject.SetActive(false);
            }
            if (other.gameObject.TryGetComponent(out BonusDummyController bonusDummy))
            {
                bonusDummy.GetHit(_damage,transform.position,_fireRate);
                gameObject.SetActive(false);
            }
        }
    }
}