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
       [SerializeField] private bool[] _occupiedGrids = new bool[9];


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
            LevelSignals.Instance.OnNewGrid += SetOccupiedGrids;
            LevelSignals.Instance.OnGridLeave += EmptyAGrid;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnGrid -= PlaceSoldiers;
            LevelSignals.Instance.OnNewGrid -= SetOccupiedGrids;
            LevelSignals.Instance.OnGridLeave -= EmptyAGrid;
        }

        private void PlaceSoldiers(List<GameObject> soldiers)
        {
            var delay = .5f;
            for (var i = 0; i < soldiers.Count; i++)
            {
                var takenSoldier = soldiers[i];
                var desiredPosition = new Vector3(grids[i].position.x, takenSoldier.transform.position.y,
                    grids[i].position.z);
                
                var shooterComponent = takenSoldier.GetComponent<ShooterController>();
                var dragNDropComponent = takenSoldier.GetComponent<DragNDropController>();
                var rb = takenSoldier.AddComponent<Rigidbody>();

                rb.isKinematic = true;
                _occupiedGrids[i] = true;
                takenSoldier.transform.DOMove(desiredPosition, 1f).SetEase(Ease.OutSine).SetDelay(delay)
                    .OnComplete(() => SetSoldierState(shooterComponent));
                dragNDropComponent.InitFirstGrid(i);
                delay += .1f;

                void SetSoldierState(ShooterController shooter)
                {
                    shooter.ChangeSoldierState(SoldierState.Idle);
                    takenSoldier.transform.SetParent(null);
                }
            }
            SetGridColliders();
        }

        private void EmptyAGrid(int index)
        {
            _occupiedGrids[index] = false;
            SetGridColliders();
        }
        private void SetOccupiedGrids(int oldIndex,int newIndex)
        {
            _occupiedGrids[oldIndex] = false;
            _occupiedGrids[newIndex] = true;
            SetGridColliders();
        }

        private void SetGridColliders()
        {
            for (var i = 0; i < grids.Length; i++)
            {
                var coll = grids[i].GetComponent<BoxCollider>();
                coll.enabled = !_occupiedGrids[i];
            }
        }

        public Vector3 GetMergePosition()
        {
            var offset = new Vector3(0, 8, -3); //offset for camera merge position
            return gridArea.position + offset;
        }
    }
}