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

        private ShooterController _shooterController;
        private BoxCollider _boxCollider;
        private int _xp;

        private void Awake()
        {
            _shooterController = GameObject.Find("Player").GetComponent<ShooterController>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            UpdateInterface();
        }

        public void IncreaseGateStats()
        {
            switch (gateMode)
            {
                case GateMode.FireRate:
                    fireRate += 00.1f;
                    break;
                case GateMode.Damage:
                    damage += 0.1f;
                    break;
                case GateMode.Range:
                    range += 0.1f;
                    break;
                case GateMode.LevelUp:
                    break;
                default:
                    break;
            }
            UpdateInterface();
        }

        public void Selection()
        {
            switch (gateMode)
            {
                case GateMode.FireRate:
                    _shooterController.FireRate = fireRate/100;
                    break;
                case GateMode.Damage:
                    _shooterController.Damage = damage;
                    break;
                case GateMode.Range:
                    _shooterController.Range = range/20;
                    break;
                case GateMode.LevelUp:
                    LevelSignals.Instance.OnXpClaimed?.Invoke(_xp);
                    break;
                default:
                    break;
            }

            _boxCollider.enabled = false;
            transform.DOMoveY(-5, goDownDuration).SetEase(Ease.InBack);
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
                    gateText.text = signFire +fireRate.ToString("0.0");
                    break; 
                case GateMode.Damage:
                    var signDamage = Mathf.Sign(damage) > 0 ? "+" : "";
                    headlineText.text = "DAMAGE";
                    gateText.text =signDamage + (damage*10).ToString("0");
                    break;
                case GateMode.Range:
                    var signRange = Mathf.Sign(range) > 0 ? "+" : "";
                    headlineText.text = "RANGE";
                    gateText.text = signRange + range.ToString("0.0");
                    break;
                case GateMode.LevelUp:
                    break;
                default:
                    break;
            }
            
        }
    }
}