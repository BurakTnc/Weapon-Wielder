using System;
using _Root.Scripts.Signals;
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
        private Animator _animator;
        private static readonly int İsStopped = Animator.StringToHash("isStopped");


        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            CoreGameSignals.Instance.OnLevelComplete += Stop;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnLevelComplete -= Stop;
        }

        private void Stop()
        {
            _isStopped = true;
            _animator.SetTrigger(İsStopped);
        }

        private void Update()
        {
            Run();
        }

        private void Run()
        {

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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StopPoint"))
            {
                Stop();
                //LevelSignals.Instance.OnEnemyKilled?.Invoke();
            }
        }
    }
}