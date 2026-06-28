using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class FadeImageControl : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private bool _isStartFadeIn = true;
        [SerializeField] private FadeType _fadeInType;

        public async void Start()
        {
            if (_isStartFadeIn)
            {
               await  FadeIn(_fadeInType);
            }
        }
        public async UniTask FadeIn(FadeType fadeType, Action callback = null)
        {
            Color startColor = TypeToColor(fadeType);

            //フェード画像の初期設定
            _fadeImage.gameObject.SetActive(true);
            _fadeImage.transform.SetAsLastSibling();
            startColor.a = 1f;
            _fadeImage.color = startColor;

            //フェードアニメーション
            await _fadeImage.DOFade(0, _fadeDuration).AsyncWaitForCompletion();
            callback?.Invoke();

            _fadeImage.gameObject.SetActive(false);
        }

        public async UniTask FadeOut(FadeType fadeType, Action callback = null)
        {
            Color endColor = TypeToColor(fadeType);

            //フェード画像の初期設定
            _fadeImage.gameObject.SetActive(true);
            _fadeImage.transform.SetAsLastSibling();
            endColor.a = 0f;
            _fadeImage.color = endColor;
            endColor.a = 1f;
            await _fadeImage.DOColor(endColor, _fadeDuration).AsyncWaitForCompletion();
            callback?.Invoke();
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
                    Debug.LogError("FadeType Error");
                    return new Color(0,0,0);
            }
        }
    }
    public enum FadeType
    {
        Black,
        White,
    }
}