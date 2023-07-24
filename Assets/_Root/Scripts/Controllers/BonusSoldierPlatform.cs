using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace _Root.Scripts.Controllers
{
    public class BonusSoldierPlatform : MonoBehaviour
    {
        [SerializeField] private float decentMultiplier;

        private BoxCollider _collider;
        private Transform _soldier;
        private Vector3 _startScale;

        private void Awake()
        {
            _soldier = transform.GetChild(0);
            _collider = GetComponent<BoxCollider>();
            _startScale = transform.localScale;
        }

        public void GetHit(float shakeLength)
        {
            transform.DOKill();
            transform.localScale = _startScale;
            transform.DOShakeScale(shakeLength, new Vector3(.1f,0,.1f), 10, 100);
            transform.position -= Vector3.up * decentMultiplier;
            
            if(transform.position.y>-1.9f)
                return;
            SetFreeTheSoldier();
        }

        private void SetFreeTheSoldier()
        {
            _soldier.tag = "Soldier";
            _soldier.SetParent(null);
            _soldier.transform.localScale = Vector3.one;
            _collider.enabled = false;
            // Destroy(gameObject);
        }
    }
}