using System;
using System.Collections.Generic;
using _Root.Scripts.Enums;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class GangController : MonoBehaviour
    {
        public static GangController Instance;
        
        [SerializeField] private List<Transform> soldierPositions = new List<Transform>();
        [SerializeField] private List<GameObject> soldiers = new List<GameObject>();

        private int _gangSize;


        private void Awake()
        {
            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            LevelSignals.Instance.OnNewGangMember += AddNewSoldier;
            LevelSignals.Instance.OnBuySoldier += BuyNewSoldier;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnNewGangMember -= AddNewSoldier;
            LevelSignals.Instance.OnBuySoldier -= BuyNewSoldier;
        }

        private void BuyNewSoldier()
        {
            _gangSize++;
        }
        private void AddNewSoldier(Transform newSoldier)
        {
            if (_gangSize >=7)
            {
                return;
            }
            var shooterComp = newSoldier.GetComponent<ShooterController>();
            newSoldier.transform.tag = "Gang";
            newSoldier.SetParent(transform);
            soldiers.Add(newSoldier.gameObject);
            shooterComp.ChangeSoldierState(SoldierState.Run,true);
            newSoldier.DORotate(Vector3.zero, 1);
            newSoldier.DOLocalMove(soldierPositions[_gangSize].localPosition, 1).SetEase(Ease.OutSine)
                .OnComplete(() => ActivateSoldier(shooterComp));
            _gangSize++;
        }

        private void ActivateSoldier(ShooterController shooter)
        {
            shooter.Activate();
        }

        public void EliminateSoldier()
        {
            _gangSize--;
            if (_gangSize <= -1) 
            {
                CoreGameSignals.Instance.OnLevelComplete?.Invoke();
            }
        }
        public List<GameObject> GetGangList()
        {
            return soldiers;
        }
    }
}