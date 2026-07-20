using DG.Tweening;
using TMPro;
using UnityEngine;
namespace InGame.UI
{
    /// <summary>
    /// ”»’èUI‚ÌView
    /// </summary>
    public class JudgeUIView : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _popScale = 1.2f;
        [SerializeField] private float _popDuration = 0.08f;
        [SerializeField] private float _returnDuration = 0.12f;
        [SerializeField] private float _fadeDelay = 0.25f;
        [SerializeField] private float _fadeDuration = 0.25f;
        [SerializeField] private float _defaultSize = 1f;

        private Sequence _sequence;
        private void Awake()
        {
            _sequence = DOTween.Sequence()
                .Append(_text.rectTransform.DOScale(_popScale, _popDuration).SetEase(Ease.OutBack))
                .Append(_text.rectTransform.DOScale(_defaultSize, _returnDuration).SetEase(Ease.OutQuad))
                .Join(_text.DOFade(0f, _fadeDuration).SetDelay(_fadeDelay))
                .SetAutoKill(false)
                .Pause();
        }

        private void OnEnable()
        {
            _text.rectTransform.localScale = Vector3.one;
            _text.alpha = 1f;
        }

        public void PlayAnimation(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
            _sequence.Restart();
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}