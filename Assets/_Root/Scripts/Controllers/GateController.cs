using System;
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
        [SerializeField] private int damage;
        [SerializeField] private GameObject mainPotion;

        private ShooterController _shooterController;
        private int _xp;

        private void Awake()
        {
            _shooterController = GameObject.Find("Player").GetComponent<ShooterController>();
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
                    fireRate += 0.1f;
                    break;
                case GateMode.Damage:
                    damage += 1;
                    break;
                case GateMode.Range:
                    range += 1f;
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
                    _shooterController.FireRate = fireRate/10;
                    break;
                case GateMode.Damage:
                    _shooterController.Damage = damage;
                    break;
                case GateMode.Range:
                    _shooterController.Range = range/10;
                    break;
                case GateMode.LevelUp:
                    LevelSignals.Instance.OnXpClaimed?.Invoke(_xp);
                    break;
                default:
                    break;
            }
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
                    gateText.text = "+" +(fireRate).ToString("0.0");
                    break;
                case GateMode.Damage:
                    gateText.text = "+" + damage;
                    break;
                case GateMode.Range:
                    gateText.text = "+" + range;
                    break;
                case GateMode.LevelUp:
                    break;
                default:
                    break;
            }
            
        }
    }
}