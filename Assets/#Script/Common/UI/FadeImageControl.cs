using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// シーン切り替え時のフェード
    /// </summary>
    public class FadeImageControl : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private bool _isStartFadeIn = true;
        [SerializeField] private FadeType _fadeInType;

        private Tween _tween;

        public async void Start()
        {
            if (_isStartFadeIn)
            {
                await FadeIn(_fadeInType);
            }
        }

        public async UniTask FadeIn(FadeType fadeType, Action callback = null)
        {
            _tween?.Kill();

            Color startColor = TypeToColor(fadeType);
            SetupFade(startColor, 1f);

            _tween = _fadeImage.DOFade(0, _fadeDuration)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    _fadeImage.gameObject.SetActive(false);
                });

            await _tween.AsyncWaitForCompletion();
        }

        public async UniTask FadeOut(FadeType fadeType, Action callback = null)
        {
            _tween?.Kill();

            Color endColor = TypeToColor(fadeType);
            SetupFade(endColor, 0f);

            _tween = _fadeImage.DOFade(1f, _fadeDuration)
                   .OnComplete(() => callback?.Invoke());

            await _tween.AsyncWaitForCompletion();
        }

        private void SetupFade(Color color, float alpha)
        {
            _fadeImage.gameObject.SetActive(true);
            _fadeImage.transform.SetAsLastSibling();
            color.a = alpha;
            _fadeImage.color = color;
        }

        private Color TypeToColor(FadeType fadeType)
        {
            switch (fadeType)
            {
                case FadeType.Black:
                    return Color.black;
                case FadeType.White:
                    return Color.white;
                default:
                    Debug.LogError($"[FadeImage] 未対応のFadeTypeです : {fadeType}");
                    return new Color(0, 0, 0);
            }
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }

    public enum FadeType
    {
        Black,
        White,
    }
}