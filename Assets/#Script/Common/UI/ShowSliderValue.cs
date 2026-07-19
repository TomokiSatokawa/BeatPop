using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// スライダーの値を表示する
    /// </summary>
    public class ShowSliderValue : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;
        void Start()
        {
            if(_slider == null)
            {
                Debug.LogError("[SliderValue] Sliderが設定されていません");
                enabled = false;
                return;
            }

            if (_text == null)
            {
                Debug.LogError("[SliderValue] Textが設定されていません");
                enabled = false;
                return;
            }

            _slider.onValueChanged.AddListener(OnChangeValue);
            OnChangeValue(_slider.value);
        }

        public void OnChangeValue(float value)
        {
            _text.text = value.ToString();
        }
    }
}