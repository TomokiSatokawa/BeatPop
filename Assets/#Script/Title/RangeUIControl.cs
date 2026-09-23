using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.Common
{

    public class RangeUIControl : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _minInput;
        [SerializeField] private TMP_InputField _maxInput;
        [SerializeField] private Slider _minSlider;
        [SerializeField] private Slider _maxSlider;
        [SerializeField] private TextMeshProUGUI _minText;
        [SerializeField] private TextMeshProUGUI _maxText;
        [SerializeField] private RectTransform _fillImage;
        [SerializeField] private RectTransform _minHandleRect;
        [SerializeField] private RectTransform _maxHandleRect;

        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private int _rangeValueMin;
        [SerializeField] private int _rangeValueMax;
        [SerializeField] private int _minRange;
        public int RangeValueMin
        {
            get
            {
                if (_rangeValueMin <= _minValue)
                {
                    return int.MinValue;
                }
                return _rangeValueMin;
            }
        }
        public int RangeValueMax
        {
            get
            {
                if (_rangeValueMax >= _maxValue)
                {
                    return int.MaxValue;
                }
                return _rangeValueMax;
            }
        }
        // Update is called once per frame  
        void Start()
        {
            if (_minSlider != null)
            {
                _minSlider.maxValue = _maxValue;
                _minSlider.minValue = _minValue;
                _minSlider.onValueChanged.AddListener(x => ChangeMinValue(Mathf.RoundToInt(x)));
            }
            if (_maxSlider != null)
            {
                _maxSlider.maxValue = _maxValue;
                _maxSlider.minValue = _minValue;
                _maxSlider.onValueChanged.AddListener(x => ChangeMaxValue(Mathf.RoundToInt(x)));
            }
            if (_minInput != null)
            {
                _minInput.onEndEdit.AddListener(x => ChangeMinValue(int.Parse(x)));
            }
            if (_maxInput != null)
            {
                _maxInput.onEndEdit.AddListener(x => ChangeMaxValue(int.Parse(x)));
            }

            ChangeMinValue(_rangeValueMin);
            ChangeMaxValue(_rangeValueMax);
        }
        public void ChangeMinValue(int value)
        {
            int minValue = ClampRangeValue(value);

            if (minValue > RangeValueMax - _minRange)
            {
                ChangeMinValue(RangeValueMax - _minRange);
                return;
            }

            if (_minInput != null)
            {
                _minInput.text = minValue.ToString();
            }
            if (_minSlider != null)
            {
                _minSlider.value = minValue;
            }
            if (_minText != null)
            {
                _minText.text = minValue.ToString() + (minValue == _minValue ? "ˆÈ‰º" : "");
            }
            _rangeValueMin = minValue;

            UpdateFill();
        }

        public void ChangeMaxValue(int value)
        {
            int maxValue = ClampRangeValue(value);

            if (maxValue < RangeValueMin + _minRange)
            {
                ChangeMaxValue(RangeValueMin + _minRange);
                return;
            }

            if (_maxInput != null)
            {
                _maxInput.text = maxValue.ToString();
            }
            if (_maxSlider != null)
            {
                _maxSlider.value = maxValue;
            }
            if (_minText != null)
            {
                _maxText.text = maxValue.ToString() + (maxValue == _maxValue ? "ˆÈã" : "");
            }
            _rangeValueMax = maxValue;

            UpdateFill();
        }

        private int ClampRangeValue(int value)
        {
            return Mathf.Clamp(value, _minValue, _maxValue);
        }

        private void UpdateFill()
        {
            if (_fillImage == null || _minSlider == null || _maxSlider == null)
            {
                return;
            }

            RectTransform sliderRect = _minSlider.GetComponent<RectTransform>();

            float minNormalized = Mathf.InverseLerp(
                _minSlider.minValue,
                _minSlider.maxValue,
                _minSlider.value);

            float maxNormalized = Mathf.InverseLerp(
                _maxSlider.minValue,
                _maxSlider.maxValue,
                _maxSlider.value);

            float width = sliderRect.rect.width;

            float minX = Mathf.Lerp(-width * 0.5f, width * 0.5f, minNormalized);
            float maxX = Mathf.Lerp(-width * 0.5f, width * 0.5f, maxNormalized);

            float left = Mathf.Min(minX, maxX);
            float right = Mathf.Max(minX, maxX);

            _fillImage.anchoredPosition = new Vector2(
                (left + right) * 0.5f,
                _fillImage.anchoredPosition.y);

            _fillImage.sizeDelta = new Vector2(
                right - left,
                _fillImage.sizeDelta.y);
        }
    }
}