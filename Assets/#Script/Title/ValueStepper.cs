using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title
{

    public class ValueStepper : MonoBehaviour
    {
        [SerializeField] private StepInfo[] _steps;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private float _startValue;
        [SerializeField] private int _displayDigitCount;
        [SerializeField] private TextMeshProUGUI _textValue;
        [SerializeField] private UnityEvent<float> _onValueChange;

        private float _value;
        public float Value => _value;
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public float StartValue => _startValue;

        public void Start()
        {
            foreach (StepInfo step in _steps)
            {
                step.Initialize(OnClick);
            }

            SetValue(_startValue);
        }

        public void SetInterpolationFactor(float factor)
        {
            SetValue(Mathf.Lerp(_minValue, _maxValue, factor));
        }

        public void SetValue(float value)
        {
            _value = Mathf.Clamp(value, _minValue, _maxValue);
            _textValue.text = _value.ToString($"N{_displayDigitCount}");
            UpdateInteractable();
        }

        private void OnClick(float amount)
        {
            SetValue(_value + amount);
            _onValueChange?.Invoke(_value);
        }

        private void UpdateInteractable()
        {
            foreach (var step in _steps)
            {
                step.AddButtonInteractable(IsClamp(_value + step.Amount));
                step.RemoveButtonInteractable(IsClamp(_value - step.Amount));
            }
        }

        private bool IsClamp(float value)
        {
            return value <= _maxValue && value >= _minValue;
        }

        [System.Serializable]
        public class StepInfo
        {
            [SerializeField] private Button _addButton;
            [SerializeField] private TextMeshProUGUI _addButtonText;
            [SerializeField] private Button _removeButton;
            [SerializeField] private TextMeshProUGUI _removeButtonText;
            [SerializeField] private float _amount;

            public float Amount => _amount;
            public void AddButtonInteractable(bool b) => _addButton.interactable = b;
            public void RemoveButtonInteractable(bool b) => _removeButton.interactable = b;

            public void Initialize(Action<float> onClick)
            {
                _addButtonText.text = $"+{_amount}";
                _removeButtonText.text = $"-{_amount}";

                _addButton.onClick.AddListener(() => onClick.Invoke(_amount));
                _removeButton.onClick.AddListener(() => onClick.Invoke(-_amount));
            }


        }

        public static float GetInterpolationFactor(ValueStepper valueStepper, float value)
        {
            return Mathf.InverseLerp(
     valueStepper.MinValue,
     valueStepper.MaxValue,
     value);
        }
    }
}