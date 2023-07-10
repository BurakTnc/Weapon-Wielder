using System;
using System.Collections;
using System.Collections.Generic;
using _Root.Scripts.Enums;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Transform[] grids;
        [SerializeField] private Transform gridArea;
        private readonly bool[] _occupiedGrids = new bool[9];


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
            LevelSignals.Instance.OnGrid += PlaceSoldiers;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnGrid -= PlaceSoldiers;
        }

        private void PlaceSoldiers(List<GameObject> soldiers)
        {
            var delay = .5f;
            for (var i = 0; i < soldiers.Count; i++)
            {
                var takenSoldier = soldiers[i];
                var desiredPosition = new Vector3(grids[i].position.x, takenSoldier.transform.position.y,
                    grids[i].position.z);

                _occupiedGrids[i] = true;
                //takenSoldier.transform.DOJump(desiredPosition,1, 1, 1f).SetDelay(delay);
                var shooterComponent = takenSoldier.GetComponent<ShooterController>();
                //shooterComponent.ChangeSoldierState(SoldierState.RunToGrid);
                takenSoldier.transform.DOMove(desiredPosition, 1f).SetEase(Ease.OutBack).SetDelay(delay)
                    .OnComplete(() => SetSoldierState(shooterComponent));
                delay += .1f;

                void SetSoldierState(ShooterController shooter)
                {
                    shooter.ChangeSoldierState(SoldierState.Idle);
                    takenSoldier.transform.SetParent(null);
                }
            }
        }

        public Vector3 GetMergePosition()
        {
            var offset = new Vector3(0, 8, -3); //offset for camera merge position
            return gridArea.position + offset;
        }
    }
}