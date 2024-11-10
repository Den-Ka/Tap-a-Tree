using System;
using UnityEngine;

namespace Tap_a_Tree.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _value;
        [Space]
        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _fill;

        public RectTransform RectTransform => transform as RectTransform;
        
        private IHealth _targetHealth;

        public float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp01(value);
                if (_container != null && _fill != null) SetFill(_value);
            }
        }

        public IHealth TargetHealth
        {
            get => _targetHealth;
            set
            {
                if (_targetHealth == value) return;
                SetTargetHealth(value);
            }
        }

        private bool IsTargetDamaged =>
            _targetHealth != null && _targetHealth.CurrentHealth < _targetHealth.MaxHealth;


        private void SetTargetHealth(IHealth health)
        {
            if (_targetHealth != null)
            {
                _targetHealth.HealthChanged -= ValidateTargetHealthValue;
                _targetHealth.Died -= OnTargetDied;
            }

            _targetHealth = health;

            if (_targetHealth != null)
            {
                _targetHealth.HealthChanged += ValidateTargetHealthValue;
                _targetHealth.Died += OnTargetDied;

                ValidateTargetHealthValue();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTargetDied()
        {
            TargetHealth = null;
        }

        private void ValidateTargetHealthValue()
        {
            Value = _targetHealth.CurrentHealth / _targetHealth.MaxHealth;
            gameObject.SetActive(IsTargetDamaged);
        }

        private void SetFill(float value)
        {
            var offset = _fill.offsetMax;
            offset.x = -_container.rect.width * (1f - _value);
            _fill.offsetMax = offset;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            SetFill(_value);
        }
#endif
    }
}