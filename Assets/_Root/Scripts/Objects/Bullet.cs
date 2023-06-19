using System;
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
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Fire(float speed,float range,float damage,float fireRate)
        {
            _damage = damage;
            _fireRate = fireRate;
            _rb.velocity = Vector3.forward * speed;
            Destroy(gameObject,range);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TargetDummyController dummy))
            {
                dummy.GetHit(_damage,transform.position,_fireRate);
                Destroy(gameObject);
            }

            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                gate.IncreaseGateStats();
                Destroy(gameObject);
            }
        }
    }
}