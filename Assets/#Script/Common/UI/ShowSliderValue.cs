using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class ShowSliderValue : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;
        void Start()
        {
            _slider.onValueChanged.AddListener(OnChangeValue);
            OnChangeValue(_slider.value);
        }

        public void OnChangeValue(float value)
        {
            _text.text = value.ToString();
        }
    }
}