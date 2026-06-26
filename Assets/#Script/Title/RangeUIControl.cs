using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RangeUIControl : MonoBehaviour
{
    [SerializeField] private TMP_InputField _minInput;
    [SerializeField] private TMP_InputField _maxInput;
    [SerializeField] private Slider _minSlider;
    [SerializeField] private Slider _maxSlider;
    [SerializeField] private TextMeshProUGUI _minText;
    [SerializeField] private TextMeshProUGUI _maxText;
    [SerializeField] private RectTransform _fillImage;

    [SerializeField] private int _minValue;
    [SerializeField] private int _maxValue;
    [SerializeField] private int _rangeValueMin;
    [SerializeField] private int _rangeValueMax;
    [SerializeField] private int _minRange;
    public int RangeValueMin => _rangeValueMin;
    public int RangeValueMax => _rangeValueMax;
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

    }
    private int ClampRangeValue(int value)
    {
        return Mathf.Clamp(value, _minValue, _maxValue);
    }
}
