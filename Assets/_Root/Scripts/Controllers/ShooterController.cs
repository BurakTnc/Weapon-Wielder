using System;
using System.Collections.Generic;
using _Root.Scripts.Enums;
using _Root.Scripts.Managers;
using _Root.Scripts.Objects;
using _Root.Scripts.ScriptableObjects;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Root.Scripts.Controllers
{
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] private GameObject[] soldiers;
        [SerializeField] private SoldierData soldierData;
        [SerializeField] private Transform shootingPosition;
        [SerializeField] private bool isCollectible;

        private float _fireRate;
        private float _range;
        private float _damage;
        private int _soldierLevel;
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
            _fireRate = soldierData.fireRate;
            _range = soldierData.range;
            _damage = soldierData.damage;
        }

        private void Update()
        {
            if(isCollectible)
                return;
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
                _fireRate = Mathf.Clamp(_fireRate, 0.03f, 2);
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
                if (_xp >= 0) 
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
            for (var i = 0; i < _soldierLevel+1; i++)
            {
                var bullet = PoolManager.Instance.GetPooledObject(PooledObjectType.Bullet);

                bullet.transform.position = shootingPosition.position;
                
                if (bullet.gameObject.TryGetComponent(out Bullet firedBullet))
                {
                    firedBullet.Fire(7, Range, Damage, FireRate);
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
            _xp = 0;
            _soldierLevel++;
            ChangeSoldier();
            UIManager.Instance.UpdateXp(_xp);
        }

        private void ChangeSoldier()
        {
            var chance = Random.Range(0, 2);
            if(chance>0)
                return;
            
            for (var i = 0; i < soldiers.Length; i++)
            {
                if (i == _soldierLevel - 1)
                {
                    soldiers[i].SetActive(false);
                }
                else
                {
                    soldiers[i].SetActive(false);
                }
                
            }
            
            var current = soldiers[_soldierLevel];
            var scale = current.transform.localScale;
            current.transform.localScale=Vector3.zero;
            current.SetActive(true);
            current.transform.DOScale(scale, .5f).SetEase(Ease.OutBack);
            current.GetComponent<ShooterController>().Activate();
            //transform.gameObject.SetActive(false);
        }

        public void Activate()
        {
            transform.tag = "Untagged";
            isCollectible = false;
            OnGameStart();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                gate.Selection(this);
            }

            if (other.gameObject.CompareTag("Soldier"))
            {
                LevelSignals.Instance.OnNewGangMember?.Invoke(other.transform.root);
            }
        }
    }
}