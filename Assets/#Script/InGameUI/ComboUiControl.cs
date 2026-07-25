using DG.Tweening;
using TMPro;
using UnityEngine;

namespace InGame.UI
{
    /// <summary>
    /// ƒRƒ“ƒ{UI‚ÌView
    /// </summary>
    public class ComboUIControl : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] private Transform _parentObject;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private TextMeshProUGUI _afterText;
        [SerializeField] private TextMeshProUGUI _subText;
        [SerializeField] private TMP_ColorGradient _perfectColor;
        [SerializeField] private TMP_ColorGradient _normalColor;
        [Header("Size")]
        [SerializeField] private float _maxSize;
        [SerializeField] private float _minSize;
        [Header("Duration")]
        [SerializeField] private float _afterMaxSize;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _expandRatio = 0.7f;
        [SerializeField] private float _shrinkRatio = 0.3f;
        [SerializeField] private float _afterFadeRatio = 0.5f;

        private Sequence _sequence;
        private float _defaultSize;

        private void Start()
        {
            _defaultSize = _parentObject.localScale.x;
            CreateAnimation();
        }

        private void CreateAnimation()
        {
            _sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Pause();

            _sequence.AppendCallback(() =>
            {
                _parentObject.gameObject.SetActive(true);
                _parentObject.localScale = Vector3.one * _minSize;
                _afterText.transform.localScale = Vector3.one * _minSize;
                _afterText.color = Color.black;

            });

            _sequence.Append(_parentObject.DOScale(_maxSize, _animationDuration * _expandRatio).From(_minSize));
            _sequence.Join(_afterText.transform.DOScale(_afterMaxSize, _animationDuration * _afterFadeRatio).From(_minSize));
            _sequence.Join(_afterText.DOFade(0, _animationDuration));

            _sequence.Append(_parentObject.DOScale(_defaultSize, _animationDuration * _shrinkRatio));
            _sequence.ForceInit();

        }

        public void PlayComboAnimation(int count,bool isPerfect)
        {
            _comboText.text = count.ToString();
            _afterText.text = count.ToString();

            _comboText.colorGradientPreset = isPerfect ? _perfectColor : _normalColor;
            _subText.colorGradientPreset = isPerfect ? _perfectColor : _normalColor;

            _sequence.Restart(true);
        }

        public void Hide()
        {
            _parentObject.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}