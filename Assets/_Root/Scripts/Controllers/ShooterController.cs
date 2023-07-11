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
        private bool _isAiming;
        private Animator _animator;
        private DragNDropController _dragNDropController;
        private Transform _crossHair;
        
        private static readonly int İsRunning = Animator.StringToHash("isRunning");
        private static readonly int İsShooting = Animator.StringToHash("isShooting");
        private static readonly int RunToGrid = Animator.StringToHash("RunToGrid");

        private void Awake()
        {
            _dragNDropController = GetComponent<DragNDropController>();
            _crossHair = GameObject.Find("CrossHair").transform;
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
            LevelSignals.Instance.OnFight -= Fight;
            LevelSignals.Instance.OnFight -= RotationFix;
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnGameStart += OnGameStart;
            CoreGameSignals.Instance.OnLevelComplete += OnGameEnd;
            LevelSignals.Instance.OnXpClaimed += CollectXp;
            LevelSignals.Instance.OnStop += OnGameEnd;
            LevelSignals.Instance.OnFight += Fight;
            LevelSignals.Instance.OnFight += RotationFix;
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
            TakeAim();
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
                    var rawDirection = transform.forward * (0.03f);
                    var fightingDirection = transform.forward * (0.05f);
                    var desiredDirection = _isAiming ? fightingDirection : rawDirection;
                    firedBullet.Fire(desiredDirection, Range, Damage, FireRate);
                }
            }
        }

        private void TakeAim()
        {
            if(!_isAiming)
                return;
            
            transform.LookAt(_crossHair, Vector3.up);
        }

        private void RotationFix()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                switch (i)
                {
                  case 0:
                      var fixedRot=Quaternion.Euler(0,20,0);
                      transform.GetChild(i).localRotation = fixedRot;
                      break;
                      
                  case 1:
                      var fixedRot1=Quaternion.Euler(0,20,0);
                      transform.GetChild(i).localRotation = fixedRot1;
                      break;
                  case 2:
                      var fixedRot2=Quaternion.Euler(0,20,0);
                      transform.GetChild(i).localRotation = fixedRot2;
                      break;
                  case 3:
                      var fixedRot3=Quaternion.Euler(0,40,0);
                      transform.GetChild(i).localRotation = fixedRot3;
                      break;
                  case 4:
                      var fixedRot4=Quaternion.Euler(0,50,0);
                      transform.GetChild(i).localRotation = fixedRot4;
                      break;
                }
            }
        
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

        private void Fight()
        {
            _isAiming = true;
            _isRunning = true;
            ChangeSoldierState(SoldierState.Shoot);
        }
        public void Activate()
        {
            transform.tag = "Gang";
            isCollectible = false;
            OnGameStart();
        }

        private void CheckMergeConditions(int takenLevel,GameObject takenSoldier)
        {
            if(takenLevel>=4)
                return;
            
            if (takenLevel == _soldierLevel)
            {
                Merge(takenSoldier);
            }
        }

        private void Merge(GameObject takenSoldier)
        {
            var dragController = takenSoldier.GetComponent<DragNDropController>();
            
            dragController.LeaveGridByMerge();
            MergeLevelUp();
            Destroy(takenSoldier);
        }

        private void MergeLevelUp()
        {
            _soldierLevel++;
            ChangeSoldier();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                gate.Selection(this);
            }

            if (other.gameObject.CompareTag("Soldier"))
            {
                LevelSignals.Instance.OnNewGangMember?.Invoke(other.transform);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Grid"))
            {
                if(!canMerge)
                    return;
                _dragNDropController.PlaceSoldier(other.transform.position,other.transform);
            }
            if (other.gameObject.CompareTag("Gang"))
            {
                if(!canMerge)
                    return;
                other.GetComponent<ShooterController>().CheckMergeConditions(_soldierLevel, transform.gameObject);
            }
        }
        
    }
}