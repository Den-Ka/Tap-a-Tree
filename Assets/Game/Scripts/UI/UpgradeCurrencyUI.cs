using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tap_a_Tree.UI
{
    public class UpgradeCurrencyUI : MonoBehaviour
    {

        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;

        [FormerlySerializedAs("_canUpgrade")] [SerializeField] private bool _canPurchase = true;
        [SerializeField] private int _value = 100;
        
        [SerializeField] private Color _affordableColor = Color.black;
        [SerializeField] private Color _unaffordableColor = Color.red;

        public Sprite Icon
        {
            set => _iconImage.sprite = value;
        }
        public int Value
        {
            set
            {
                _value = value;
                ValidateTextValue();
            }
        }

        public bool CanPurchase
        {
            set
            {
                _canPurchase = value;
                ValidateTextColor();
            }
        }

        public void ColorText(TextMeshProUGUI text)
        {
            text.color = _canPurchase ? _affordableColor : _unaffordableColor;
        }
        
        private void ValidateTextColor()
        {
            if (_amountText == null) return;

            ColorText(_amountText);
        }

        private void ValidateTextValue()
        {
            if(_amountText == null) return;
            
            _amountText.text = _value.ToString("N0");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateTextColor();
            ValidateTextValue();
        }
#endif
    }
}