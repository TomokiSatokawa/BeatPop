using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.Custom
{
    public class PatternUIControl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _patternName;
        [SerializeField] private Image _selectLine;
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _setPattern;
        private PatternJsonData _patternData;
        public PatternJsonData PatternData => _patternData;
        public void SetData(PatternJsonData pattern, Action<PatternUIControl> onSelect)
        {
            _patternData = pattern;
            _selectButton.onClick.AddListener(() =>
            {
                onSelect?.Invoke(this);
            });
            _patternName.text = _patternData.PatternName;
            OnDeselect();
        }
        public void OnSelect()
        {
            _selectLine.gameObject.SetActive(true);
        }
        public void OnDeselect()
        {
            _selectLine.gameObject.SetActive(false);
        }
        public void ShowSetPattern(bool active)
        {
            _setPattern.SetActive(active);
        }
    }
}