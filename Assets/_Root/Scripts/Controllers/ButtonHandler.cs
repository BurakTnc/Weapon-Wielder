using System;
using _Root.Scripts.Managers;
using _Root.Scripts.ScriptableObjects;
using _Root.Scripts.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Controllers
{
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button damageButton, fireRateButton, rangeButton, addSoldierButton;
        [SerializeField] private int[] damagePrice, fireRatePrice, rangePrice, addSoldierPrice;
        [SerializeField] private SoldierData soldierData;

        [SerializeField] private TextMeshProUGUI damagePriceText,
            damageLevelText,
            fireRatePriceText,
            fireRateLevelText,
            rangeLevelText,
            rangePriceText,
            addSoldierPriceText;

        private int _damageLevel, _fireRateLevel, _rangeLevel, _addSoldierLevel;


        private void Awake()
        {
            GetValues();
        }

        private void Start()
        {
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

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
            CoreGameSignals.Instance.OnSave += Save;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
        }
        private void GetValues()
        {
            _damageLevel = PlayerPrefs.GetInt("damageLevel", 0);
            _fireRateLevel = PlayerPrefs.GetInt("fireRateLevel", 0);
            _rangeLevel = PlayerPrefs.GetInt("rangeLevel", 0);
            _addSoldierLevel = PlayerPrefs.GetInt("addSoldierLevel", 0);
            fireRatePrice = damagePrice;
            rangePrice = damagePrice;
            addSoldierPrice = damagePrice;
        }
        private void Save()
        {
            PlayerPrefs.SetInt("damageLevel",_damageLevel);
            PlayerPrefs.SetInt("fireRateLevel",_fireRateLevel);
            PlayerPrefs.SetInt("rangeLevel",_rangeLevel);
            PlayerPrefs.SetInt("addSoldierLevel", _addSoldierLevel);
            CheckButtonConditions();
        }
        private void CheckButtonConditions()
        {
            var money = GameManager.Instance.money;

            damageButton.interactable = money >= damagePrice[_damageLevel];
            fireRateButton.interactable = money >= fireRatePrice[_fireRateLevel];
            rangeButton.interactable = money >= rangePrice[_rangeLevel];
            addSoldierButton.interactable = money >= addSoldierPrice[_addSoldierLevel];

            UpdateInterface();
        }

        private void UpdateInterface()
        {
            damageLevelText.text = (_damageLevel + 1).ToString();
            damagePriceText.text = damagePrice[_damageLevel] + "$";

            fireRateLevelText.text = (_fireRateLevel + 1).ToString();
            fireRatePriceText.text = fireRatePrice[_fireRateLevel] + "$";

            rangeLevelText.text = (_rangeLevel + 1).ToString();
            rangePriceText.text = rangePrice[_rangeLevel] + "$";

            //addSoldierLevelText.text = (_addSoldierLevel + 1).ToString();
            addSoldierPriceText.text = addSoldierPrice[_addSoldierLevel] + "$";
        }

        public void DamageButton()
        {
            LevelSignals.Instance.OnUpgrade?.Invoke();
            soldierData.damage += 1;
            GameManager.Instance.money -= damagePrice[_damageLevel];
            _damageLevel++;
            if (_damageLevel >20)
            {
                _damageLevel = 20;
            }
            Save();
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

        public void FireRateButton()
        {
            soldierData.fireRate -= 0.015f;
            LevelSignals.Instance.OnUpgrade?.Invoke();
            GameManager.Instance.money -= fireRatePrice[_fireRateLevel];
            _fireRateLevel++;
            if (_fireRateLevel >20)
            {
                _fireRateLevel = 20;
            }
            Save();
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

        public void RangeButton()
        {
            soldierData.range += 0.2f;
            LevelSignals.Instance.OnUpgrade?.Invoke();
            GameManager.Instance.money -= damagePrice[_damageLevel];
            _damageLevel++;
            if (_rangeLevel >20)
            {
                _rangeLevel = 20;
            }
            Save();
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

        public void AddSoldierButton()
        {
            GameManager.Instance.money -= addSoldierPrice[_addSoldierLevel];
            _addSoldierLevel++;
            Save();
            CoreGameSignals.Instance.OnSave?.Invoke();
        }
    }
}
