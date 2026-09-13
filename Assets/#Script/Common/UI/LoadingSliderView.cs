using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// ロードスライダーのView
    /// </summary>
    public class LoadingSliderView : MonoBehaviour
    {
        [SerializeField] private Image _sliderImage;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void UpdateValue(float value)
        {
            _sliderImage.fillAmount = value;
            _valueText.text = (value * 100f).ToString("N0") + "%";
        }
    }
}