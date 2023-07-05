using System;
using System.Collections.Generic;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class GangController : MonoBehaviour
    {
        [SerializeField] private List<Transform> soldierPositions = new List<Transform>();
        [SerializeField] private List<GameObject> soldiers = new List<GameObject>();

        private int _gangSize;

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
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnNewGangMember -= AddNewSoldier;
        }

        private void AddNewSoldier(Transform newSoldier)
        {
            var shooterComp = newSoldier.GetComponent<ShooterController>();
            
            newSoldier.SetParent(transform);
            soldiers.Add(newSoldier.gameObject);
            newSoldier.DORotate(Vector3.zero, 1);
            newSoldier.DOLocalMove(soldierPositions[_gangSize].localPosition, 1).SetEase(Ease.OutSine)
                .OnComplete(() => ActivateSoldier(shooterComp));
            _gangSize++;
        }

        private void ActivateSoldier(ShooterController shooter)
        {
            shooter.Activate();
        }

        public List<GameObject> GetGangList()
        {
            return soldiers;
        }
    }
}