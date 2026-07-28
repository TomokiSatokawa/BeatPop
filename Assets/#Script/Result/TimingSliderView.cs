using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Result.UI
{
    /// <summary>
    /// タイミングスライダー
    /// </summary>
    public class TimingSliderView : MonoBehaviour
    {
        [Header("Slider")]
        [SerializeField] private Image _fastSlider;
        [SerializeField] private Image _lateSlider;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _fastValue;
        [SerializeField] private TextMeshProUGUI _lateValue;

        [Header("Animation")]
        [SerializeField] private float _waitTime = 0.5f;
        [SerializeField] private float _sliderDuration = 0.65f;
        [SerializeField] private float _valueDuration = 0.8f;
        [SerializeField] private float _fadeDuration = 0.25f;

        [SerializeField] private Ease _sliderEase = Ease.OutQuart;
        [SerializeField] private Ease _valueEase = Ease.OutQuad;

        [SerializeField, Range(0f, 1f)]
        private float _sliderStartAlpha = 0.45f;

        private Sequence _sequence;

        public void OnAnimation(int fastCount, int lateCount)
        {
            _sequence?.Kill();

            _sequence = DOTween.Sequence();

            Initialize();

            int total = fastCount + lateCount;

            _sequence.AppendInterval(_waitTime);
            _sequence.AppendInterval(0);


            float fastFill = GetFillAmount(fastCount, total);
            float lateFill = GetFillAmount(lateCount, total);

            if (total == 0)
            {
                fastFill = 0.5f;
                lateFill = 0.5f;
            }

            PlaySlider(_fastSlider, _fastValue, fastCount, fastFill);
            PlaySlider(_lateSlider, _lateValue, lateCount, lateFill);

        }

        private void Initialize()
        {
            InitializeSlider(_fastSlider);
            InitializeSlider(_lateSlider);

            _fastValue.text = "0";
            _lateValue.text = "0";
        }

        private void InitializeSlider(Image slider)
        {
            slider.fillAmount = 0f;

            var color = slider.color;
            color.a = _sliderStartAlpha;
            slider.color = color;
        }

        private void PlaySlider(Image slider, TextMeshProUGUI valueText, int value, float fillAmount)
        {
            if (fillAmount <= 0)
                return;

            _sequence.Join(
                slider
                    .DOFillAmount(fillAmount, _sliderDuration * fillAmount)
                    .SetEase(_sliderEase));

            _sequence.Join(
                slider
                    .DOFade(1f, _fadeDuration));

            _sequence.Join(
                DOVirtual.Int(
                        0,
                        value,
                        _valueDuration * fillAmount,
                        x => valueText.text = x.ToString())
                    .SetEase(_valueEase));
        }

        private static float GetFillAmount(int value, int total)
        {
            return total <= 0 ? 0f : (float)value / total;
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}