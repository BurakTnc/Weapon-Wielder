using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace _Root.Scripts.Controllers
{
    public class BonusSoldierPlatform : MonoBehaviour
    {
        [SerializeField] private float decentMultiplier;
        [SerializeField] private Material activationMaterial;

        private BoxCollider _collider;
        private Transform _soldier;
        private Vector3 _startScale;
        private MeshRenderer _renderer;

        private void Awake()
        {
            _soldier = transform.GetChild(0);
            _collider = GetComponent<BoxCollider>();
            _startScale = transform.localScale;
            _renderer = GetComponent<MeshRenderer>();
        }

        public void GetHit(float shakeLength)
        {
            transform.DOKill();
            transform.localScale = _startScale;
            transform.DOShakeScale(shakeLength, new Vector3(.2f,0,0f), 10, 100,true);
            transform.position -= Vector3.up * decentMultiplier;
            
            if(transform.position.y>-1.6f)
                return;
            SetFreeTheSoldier();
        }

        private void SetFreeTheSoldier()
        {
            _soldier.tag = "Soldier";
            _soldier.SetParent(null);
            _soldier.transform.localScale = Vector3.one;
            _collider.enabled = false;
            _renderer.material.DOColor(activationMaterial.color, .25f).SetEase(Ease.OutBack);
            // Destroy(gameObject);
        }
    }
}