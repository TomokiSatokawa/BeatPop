using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// 2Ç¬ÇÃToggleÇégÇ¡ÇƒON/OFFÇêÿÇËë÷Ç¶ÇÈ
    /// </summary>
    public class DoubleToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _onToggle;
        [SerializeField] private Toggle _offToggle;
        [SerializeField] private bool _isOn;
        [SerializeField] private UnityEvent<bool> _onValueChange;

        public bool IsOn => _isOn;

        private void Awake()
        {
            _onToggle.onValueChanged.AddListener(OnToggleChanged);
            _offToggle.onValueChanged.AddListener(OffToggleChanged);

            UpdateToggle();
        }

        private void OnDestroy()
        {
            _onToggle.onValueChanged.RemoveListener(OnToggleChanged);
            _offToggle.onValueChanged.RemoveListener(OffToggleChanged);
        }

        private void OnToggleChanged(bool isOn)
        {
            if (isOn)
            {
                SetValue(true);
            }
        }

        private void OffToggleChanged(bool isOn)
        {
            if (isOn)
            {
                SetValue(false);
            }
        }

        public void SetValue(bool value)
        {
            if (_isOn == value)
                return;

            _isOn = value;
            UpdateToggle();
            _onValueChange?.Invoke(_isOn);
        }

        private void UpdateToggle()
        {
            _onToggle.SetIsOnWithoutNotify(_isOn);
            _offToggle.SetIsOnWithoutNotify(!_isOn);
        }
    }
}