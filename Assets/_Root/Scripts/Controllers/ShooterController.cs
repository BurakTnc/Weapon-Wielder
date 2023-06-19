using System;
using System.Collections.Generic;
using _Root.Scripts.Managers;
using _Root.Scripts.Objects;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] private GameObject[] weapons;
        
        [Header("Shooting Positions")] 
        [SerializeField] private List<Transform> shootingPositions = new List<Transform>();

        [Header("Default Values")]
        [SerializeField] private float defaultFireRate;
        [SerializeField] private float defaultRange;
        [SerializeField] private int defaultDamage;
        [SerializeField] private float bulletSpeed;

        private float _fireRate;
        private float _range;
        private float _damage;
        private int _weaponLevel;
        private int _xp;
        private float _shootTimer;
        private bool _isRunning;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnGameStart -= OnGameStart;
            CoreGameSignals.Instance.OnLevelComplete -= OnGameEnd;
            LevelSignals.Instance.OnXpClaimed -= CollectXp;
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnGameStart += OnGameStart;
            CoreGameSignals.Instance.OnLevelComplete += OnGameEnd;
            LevelSignals.Instance.OnXpClaimed += CollectXp;
        }

        private void Start()
        {
            _fireRate = defaultFireRate;
            _range = defaultRange;
            _damage = defaultDamage;
        }

        private void Update()
        {
            BeginFire();
        }

        private void OnGameStart()
        {
            _isRunning = true;
        }

        private void OnGameEnd()
        {
            _isRunning = false;
        }
        public float FireRate
        {
            get => _fireRate;
            set
            {
                _fireRate -= value;
                _fireRate = Mathf.Clamp(_fireRate, 0, 2);
            } 
        }
        public float Range
        {
            get => _range;
            set => _range += value;
        }
        public float Damage
        {
            get => _damage;
            set => _damage += value;
        }
        
        public int Xp
        {
            get => _xp;
            set
            {
                _xp += value;
                if (_xp >= 100) 
                {
                    LevelUp();
                }
            } 
        }

        private void BeginFire()
        {
            if(!_isRunning)
                return;
            
            if (_shootTimer <= 0)
            {
                _shootTimer += _fireRate;
                Fire();
            }
            else
            {
                _shootTimer -= Time.deltaTime;
                _shootTimer = Mathf.Clamp(_shootTimer, 0, _fireRate);
            }
        }

        private void Fire()
        {
            for (var i = 0; i < _weaponLevel+1; i++)
            {
                var bullet = Instantiate(Resources.Load<GameObject>("Spawnables/Bullet")).transform;

                bullet.position = shootingPositions[i].position;
                
                if (bullet.gameObject.TryGetComponent(out Bullet script))
                {
                    script.Fire(bulletSpeed, Range, Damage, FireRate);
                }
            }
        }

        private void CollectXp(int earnedXp)
        {
            Xp = earnedXp;
            UIManager.Instance.UpdateXp(Xp);
        }

        private void LevelUp()
        {
            _xp -= 100;
            _weaponLevel++;
            ChangeWeapon();
            LevelSignals.Instance.OnLevelUp?.Invoke();
            UIManager.Instance.UpdateXp(_xp);
        }

        private void ChangeWeapon()
        {
            foreach (var weapon in weapons)
            {
                weapon.SetActive(false);
            }

            var current = weapons[_weaponLevel];
            var scale = current.transform.localScale;
            current.transform.localScale=Vector3.zero;
            current.SetActive(true);
            current.transform.DOScale(scale, .3f).SetEase(Ease.OutBack);
        }
    }
}