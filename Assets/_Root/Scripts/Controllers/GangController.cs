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
            newSoldier.SetParent(transform);
            // rotate
            var shooterComp = newSoldier.GetChild(0).GetComponent<ShooterController>();
            newSoldier.DOLocalMove(soldierPositions[_gangSize].localPosition, 1).SetEase(Ease.OutSine)
                .OnComplete(() => ActivateSoldier(shooterComp));
            _gangSize++;
        }

        private void ActivateSoldier(ShooterController shooter)
        {
            shooter.Activate();
        }
    }
}