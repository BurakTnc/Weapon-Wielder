using System;
using System.Collections;
using System.Collections.Generic;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Transform[] grids;

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
            var delay = 0.5f;
            for (var i = 0; i < soldiers.Count; i++)
            {
                var takenSoldier = soldiers[i];
                var desiredPosition = new Vector3(grids[i].position.x, takenSoldier.transform.position.y,
                    grids[i].position.z);

                _occupiedGrids[i] = true;
                //takenSoldier.transform.DOJump(desiredPosition, .5f, 1, 1f).SetDelay(delay);
                takenSoldier.transform.DOMove(desiredPosition, 1f).SetEase(Ease.InSine).SetDelay(delay);
                delay += 0.2f;
            }
        }
        
    }
}