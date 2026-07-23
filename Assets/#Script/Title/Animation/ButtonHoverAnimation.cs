using System;
using DG.Tweening;
using R3;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// ボタンにカーソルを合わせた時のアニメーション
    /// </summary>
    [RequireComponent(typeof(UIPointerHover))]
    public class ButtonHoverAnimation : MonoBehaviour
    {
        [SerializeField] private UIPointerHover _uIPointerHover;
        [SerializeField] private float _hoverScale = 1.1f;
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private Ease _enterEase = Ease.OutBack;
        [SerializeField] private Ease _exitEase = Ease.OutQuad;

        private Vector3 _defaultScale;
        private Tween _tween;

        public Action OnEnter;
        public Action OnExit;

        private void Awake()
        {
            _defaultScale = transform.localScale;

            if (_uIPointerHover == null)
            {
                if (!this.gameObject.TryGetComponent(out _uIPointerHover))
                {
                    _uIPointerHover = gameObject.AddComponent<UIPointerHover>();
                }
            }

            _uIPointerHover.IsPointerOver.Where(x => x).Subscribe(_ => OnPointerEnter()).AddTo(this);
            _uIPointerHover.IsPointerOver.Where(x => !x).Subscribe(_ => OnPointerExit()).AddTo(this);
        }

        private void OnPointerEnter()
        {
            _tween?.Kill();
            _tween = transform.DOScale(_defaultScale * _hoverScale, _duration)
                .SetEase(_enterEase);
            OnEnter?.Invoke();
        }

        private void OnPointerExit()
        {
            _tween?.Kill();
            _tween = transform.DOScale(_defaultScale, _duration)
                .SetEase(_exitEase);
            OnExit?.Invoke();
        }

        private void OnDisable()
        {
            _tween?.Kill();
            transform.localScale = _defaultScale;
        }
    }
}