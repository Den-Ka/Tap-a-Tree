using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tap_a_Tree.UI
{
    public class UpgradeButtonUI : MonoBehaviour
    {
        public event Action Clicked;

        [SerializeField] private Button _button;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private TextMeshProUGUI _upgradeText;
        [SerializeField] private UpgradeCurrencyUI _currency;

        private void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                if (CanUpgrade) Clicked?.Invoke();
            });
        }

        public bool CanUpgrade
        {
            get => _canUpgrade;
            set
            {
                _canUpgrade = value;
                _currency.CanPurchase = _canUpgrade;
                _currency.ColorText(_upgradeText);
            }
        }

        public string Name
        {
            set => _nameText.text = value;
        }

        public Sprite Icon
        {
            set => _iconImage.sprite = value;
        }
        
        public int Level
        {
            set => _levelText.text = $"Lvl. {value}";
        }

        public float Value
        {
            set => _valueText.text = $"{value:P0}";
        }

        public int Price
        {
            set => _currency.Value = value;
        }

        public Sprite ResourceIcon
        {
            set => _currency.Icon = value;
        }

        private bool _canUpgrade = false;


#if UNITY_EDITOR
        private void Reset()
        {
            _button = GetComponent<Button>();
        }
#endif
    }
}