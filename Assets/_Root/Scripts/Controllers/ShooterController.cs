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
        [HideInInspector] public bool canMerge;
        
        [SerializeField] private GameObject[] soldiers;
        [SerializeField] private SoldierData[] soldierData;
        [SerializeField] private Transform shootingPosition;
        [SerializeField] private bool isCollectible;

        private float _fireRate;
        private float _range;
        private float _damage;
        private int _soldierLevel;
        private int _xp;
        private float _shootTimer;
        private bool _isRunning;
        private GangController _gangController;
        private Animator _animator;
        private DragNDropController _dragNDropController;
        
        private static readonly int İsRunning = Animator.StringToHash("isRunning");
        private static readonly int İsShooting = Animator.StringToHash("isShooting");
        private static readonly int RunToGrid = Animator.StringToHash("RunToGrid");

        private void Awake()
        {
            _gangController = transform.root.GetComponent<GangController>();
            _dragNDropController = GetComponent<DragNDropController>();
        }

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
            LevelSignals.Instance.OnStop -= OnGameEnd;
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnGameStart += OnGameStart;
            CoreGameSignals.Instance.OnLevelComplete += OnGameEnd;
            LevelSignals.Instance.OnXpClaimed += CollectXp;
            LevelSignals.Instance.OnStop += OnGameEnd;
        }

        private void Start()
        {
            _fireRate = soldierData[0].fireRate;
            _range = soldierData[0].range;
            _damage = soldierData[0].damage;
            
            if (isCollectible)
                return;
            ChangeSoldierState(SoldierState.Idle);
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
            if(isCollectible)
                return;
            ChangeSoldierState(SoldierState.Run);
        }

        private void OnGameEnd()
        {
            _isRunning = false;
            ChangeSoldierState(SoldierState.Idle);
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

        public int GetSoldierLevel()
        {
            return _soldierLevel;
        }
        public void ChangeSoldierState(SoldierState state)
        {
            var animator = GetCurrentAnimator();
            switch (state)
            {
                case SoldierState.Idle:
                    animator.SetBool(İsRunning,false);
                    break;
                case SoldierState.Run:
                    animator.SetBool(İsRunning,true);
                    break;
                case SoldierState.Shoot:
                    animator.SetBool(İsShooting,true);
                    break;
                case SoldierState.RunToGrid:
                    animator.SetTrigger(RunToGrid);
                    break;
                default:
                    break;
            }
        }

        private Animator GetCurrentAnimator()
        {
            var animator = soldiers[_soldierLevel].GetComponent<Animator>();
            return animator;
        }
        private void CollectXp(int earnedXp)
        {
            Xp = earnedXp;
            UIManager.Instance.UpdateXp(Xp);
        }

        private void LevelUp()
        {
            var chance = Random.Range(0, 2);
            if(chance>0)
                return;
            _xp = 0;
            _soldierLevel++;
            ChangeSoldier();
            ChangeSoldierState(SoldierState.Run);
            UIManager.Instance.UpdateXp(_xp);
        }

        private void ChangeSoldier()
        {

            for (var i = 0; i < soldiers.Length; i++)
            {
                soldiers[i].SetActive(i == _soldierLevel);
            }
            
            var current = soldiers[_soldierLevel];
            var scale = current.transform.localScale;
            current.transform.localScale=Vector3.zero;
           // current.SetActive(true);
            current.transform.DOScale(scale, .5f).SetEase(Ease.OutBack);
           // current.GetComponent<ShooterController>().Activate();
        }

        public void Activate()
        {
            transform.tag = "Gang";
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

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Grid"))
            {
                if(!canMerge)
                    return;
                _dragNDropController.PlaceSoldier(other.transform.position);
            }
        }
    }
}