using System;
using System.Globalization;
using _Root.Scripts.Enums;
using _Root.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class GateController : MonoBehaviour
    {
        [SerializeField] private GateMode gateMode;
        [SerializeField] private TextMeshPro gateText;
        [SerializeField] private float goDownDuration;
        [SerializeField] private float fireRate;
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private GameObject mainPotion;
        [SerializeField] private TextMeshPro headlineText;

        private bool _isLocked;
        private GateVisuals _gateVisuals;
        private BoxCollider _boxCollider;
        private int _xp;

        private void Awake()
        {
            _gateVisuals = GetComponent<GateVisuals>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            UpdateInterface();
        }

        public void IncreaseGateStats()
        {
            if(_isLocked)
                return;
            switch (gateMode)
            {
                case GateMode.FireRate:
                    fireRate += 0.1f;
                    _gateVisuals.SetGateColor(fireRate>=0);
                    break;
                case GateMode.Damage:
                    damage += 0.1f;
                    _gateVisuals.SetGateColor(damage>=0);
                    break;
                case GateMode.Range:
                    _gateVisuals.SetGateColor(range>=0);
                    range += 0.1f;
                    break;
                case GateMode.LevelUp:
                    break;
                default:
                    break;
            }
            UpdateInterface();
        }

        public void Selection(ShooterController shooterController)
        {
            _isLocked = true;
            switch (gateMode)
            {
                case GateMode.FireRate:
                    shooterController.FireRate = fireRate/100;
                    break;
                case GateMode.Damage:
                    shooterController.Damage = damage;
                    break;
                case GateMode.Range:
                    shooterController.Range = range/20;
                    break;
                case GateMode.LevelUp:
                    LevelSignals.Instance.OnXpClaimed?.Invoke(_xp);
                    _boxCollider.enabled = false;
                    transform.DOMoveY(-5, goDownDuration).SetEase(Ease.InBack);
                    break;
                default:
                    break;
            }

           // _boxCollider.enabled = false;
           
        }

        public void CollectPotion()
        {
            _xp += 20;
            var growingSize = new Vector3(.01f, .01f, .01f);
            mainPotion.transform.DOScale(growingSize, .5f).SetEase(Ease.OutBack).SetRelative(true);
        }
        private void UpdateInterface()
        {
            switch (gateMode)
            {
                case GateMode.FireRate:
                    var signFire = Mathf.Sign(fireRate) > 0 ? "+" : "";
                    headlineText.text = "FIRE RATE";
                    gateText.text = signFire +(fireRate*10).ToString("0");
                    break; 
                case GateMode.Damage:
                    var signDamage = Mathf.Sign(damage) > 0 ? "+" : "";
                    headlineText.text = "DAMAGE";
                    gateText.text =signDamage + (damage*10).ToString("0");
                    break;
                case GateMode.Range:
                    var signRange = Mathf.Sign(range) > 0 ? "+" : "";
                    headlineText.text = "RANGE";
                    gateText.text = signRange + range.ToString("0");
                    break;
                case GateMode.LevelUp:
                    break;
                default:
                    break;
            }
            
        }
    }
}