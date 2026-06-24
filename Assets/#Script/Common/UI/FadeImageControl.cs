using System;
using System.Threading.Tasks;
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
        [SerializeField] private bool _onStartFadeIn;
        [SerializeField] private FadeType _fadeInType;

        public async void Start()
        {
            if (_onStartFadeIn)
            {
               await  FadeIn(_fadeInType);
            }
        }
        public async UniTask FadeIn(FadeType fadeType, Action callback = null)
        {
            _fadeImage.gameObject.SetActive(true);
            Color startColor = new Color();
            switch (fadeType)
            {
                case FadeType.Black:
                    startColor = Color.black;
                    break;
                case FadeType.White:
                    startColor = Color.white;
                    break;
                default:
                    return;
            }
            _fadeImage.transform.SetAsLastSibling();
            startColor.a = 1f;
            _fadeImage.color = startColor;
            await UniTask.Yield();
            await _fadeImage.DOFade(0, _fadeDuration).AsyncWaitForCompletion();
            callback?.Invoke();
            _fadeImage.gameObject.SetActive(false);
        }

        public async UniTask FadeOut(FadeType fadeType, Action callback = null)
        {
            _fadeImage.gameObject.SetActive(true);
            Color endColor = new Color();
            switch (fadeType)
            {
                case FadeType.Black:
                    endColor = Color.black;
                    break;
                case FadeType.White:
                    endColor = Color.white;
                    break;
                default:
                    return;
            }
            _fadeImage.transform.SetAsLastSibling();
            endColor.a = 0f;
            _fadeImage.color = endColor;
            endColor.a = 1f;
            await _fadeImage.DOColor(endColor, _fadeDuration).AsyncWaitForCompletion();
            callback?.Invoke();
        }
    }
    public enum FadeType
    {
        Black,
        White,
    }
}