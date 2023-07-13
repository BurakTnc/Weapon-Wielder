using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Controllers
{
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField] private float runSpeed;

        private Transform _targetedSoldier;
        private bool _isDead;
        private bool _isStopped;

        private void OnEnable()
        {
            //Init();
        }

        private void Update()
        {
            Run();
        }

        private void Init()
        {
            // var gangList = GangController.Instance.GetGangList();
            // var aimedSoldier = Random.Range(0, gangList.Count);
            //
            // _targetedSoldier = gangList[aimedSoldier].transform;
            // //transform.LookAt(_targetedSoldier, Vector3.up);
        }

        private void Run()
        {
            if (!_targetedSoldier)
            {
                Init();
            }
            
            if(_isDead || _isStopped)
                return;
            
            transform.position += transform.forward * (runSpeed * Time.deltaTime);
            
        }
        public void Die()
        {
            _isDead = true;
        }

        public void ResetSoldier()
        {
            _isDead = false;
            _isStopped = false;
            Init();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StopPoint"))
            {
                _isStopped = true;
            }
        }
    }
}