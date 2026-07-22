using System;
using DG.Tweening;
using R3;
using UnityEngine;

namespace Title.Common
{

    [RequireComponent(typeof(UIPointerHover))]
    public class ButtonHoverAnimation : MonoBehaviour
    {
        [SerializeField] private UIPointerHover _uIPointerHover;
        [SerializeField] private float _hoverScale = 1.1f;
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private Ease _ease = Ease.OutBack;
        public Action OnEnter;
        public Action OnExit;
        private Vector3 _defaultScale;
        private Tween _tween;

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

        public void OnPointerEnter()
        {
            _tween?.Kill();
            _tween = transform.DOScale(_defaultScale * _hoverScale, _duration)
                .SetEase(_ease);
            OnEnter?.Invoke();
        }

        public void OnPointerExit()
        {
            _tween?.Kill();
            _tween = transform.DOScale(_defaultScale, _duration)
                .SetEase(Ease.OutQuad);
            OnExit?.Invoke();
        }

        private void OnDisable()
        {
            _tween?.Kill();
            transform.localScale = _defaultScale;
        }
    }
}