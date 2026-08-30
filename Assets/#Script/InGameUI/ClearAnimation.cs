using System;
using DG.Tweening;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    /// <summary>
    /// 1種類のリザルト演出
    /// </summary>
    public class ClearAnimation : MonoBehaviour
    {
        [Header("Background")]
        [SerializeField] private Image _backGround;
        [SerializeField] private float _backgroundAlpha = 0.75f;

        [Header("Text")]
        [SerializeField] private Image _leftImage;
        [SerializeField] private Image _rightImage;
        [SerializeField] private Ease _textEase;

        [Header("Animation")]
        [SerializeField] private float _backgroundFadeDuration = 0.2f;
        [SerializeField] private float _textStartDelay = 0.1f;
        [SerializeField] private float _textFadeDuration = 0.5f;
        [SerializeField] private float _callbackWaitTime = 1f;

        private Sequence _sequence;

        private void Start()
        {
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            _backGround.gameObject.SetActive(active);
            _leftImage.gameObject.SetActive(active);
            _rightImage.gameObject.SetActive(active);
        }

        public void StartAnimation(Action onComplete = null)
        {
            _sequence?.Kill();
            SetActive(true);

            _sequence = DOTween.Sequence();

            _backGround.color = new Color(0, 0, 0, 0);

            InitText(_leftImage);
            InitText(_rightImage);

            Vector2 titleTarget = _leftImage.rectTransform.anchoredPosition;
            Vector2 subTarget = _rightImage.rectTransform.anchoredPosition;

            // 中央から開始
            _leftImage.rectTransform.anchoredPosition =
                new Vector2(0, titleTarget.y);

            _rightImage.rectTransform.anchoredPosition =
                new Vector2(0, subTarget.y);

            // 背景（アニメーション開始と同時）
            _sequence.Append(
                _backGround.DOFade(_backgroundAlpha, _backgroundFadeDuration));

            _sequence.InsertCallback(_textStartDelay, () => SoundManager.SE.PlaySE(SESoundType.StageClear));

            // タイトル（アニメーション開始から _textStartDelay 秒後）
            _sequence.Insert(
                _textStartDelay,
                _leftImage.DOFade(1, _textFadeDuration)
                .SetEase(_textEase));

            _sequence.Insert(
                _textStartDelay,
                _leftImage.rectTransform
                    .DOAnchorPosX(titleTarget.x, _textFadeDuration)
                    .SetEase(_textEase));

            // サブタイトル（アニメーション開始から _textStartDelay 秒後）
            _sequence.Insert(
                _textStartDelay,
                _rightImage.DOFade(1, _textFadeDuration)
                  .SetEase(_textEase));

            _sequence.Insert(
                _textStartDelay,
                _rightImage.rectTransform
                    .DOAnchorPosX(subTarget.x, _textFadeDuration)
                    .SetEase(_textEase));

            _sequence.InsertCallback(
                _textStartDelay + _callbackWaitTime, () => onComplete?.Invoke());
        }

        private void InitText(Image image)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
            image.rectTransform.localScale = Vector3.one;
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}