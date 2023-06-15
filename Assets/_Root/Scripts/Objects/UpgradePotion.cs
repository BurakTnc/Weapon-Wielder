using System;
using _Root.Scripts.Controllers;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Objects
{
    public class UpgradePotion : MonoBehaviour
    {
        [SerializeField] private float onBandSpeed;
        [SerializeField] private float dropDuration;
        [SerializeField] private float goingBandDuration;

        private bool _onBand;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var desiredPos = new Vector3(transform.position.x, 0, transform.position.z + 2);
            transform.DOJump(desiredPos, .4f, 1, dropDuration).SetEase(Ease.OutBounce)
                .OnComplete(GetOnBand);
        }

        private void GetOnBand()
        {
            transform.DOMoveX(-1.25f, goingBandDuration).SetEase(Ease.InSine).OnComplete(StartMoving);
        }

        private void StartMoving()
        {
            _onBand = true;
        }

        private void MoveOnBand()
        {
            if(!_onBand)
                return;
            
            transform.position += Vector3.forward * (onBandSpeed * Time.deltaTime);
        }

        private void Update()
        {
            MoveOnBand();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_onBand)
                return;
            
            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                // spawn particle
                gate.CollectPotion();
                Destroy(gameObject);

            }
        }
    }
}