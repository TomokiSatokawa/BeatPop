using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System;

namespace InGame.UI
{
    /// <summary>
    /// ƒNƒŠƒAŽž‚ÌUI‚ÌView
    /// </summary>
    public class ClearAnimation : MonoBehaviour
    {
        [SerializeField] private Image _mainVisualMask;
        [SerializeField] private SceneTransition _sceneLoad;
        [SerializeField] private float _maxWidth;
        [SerializeField] private float _animationDuration;

        private void Start()
        {
            _mainVisualMask.gameObject.SetActive(false);
        }

        public void StartAnimation(Action onComplete  = null)
        {
            RectTransform rectTransform = _mainVisualMask.rectTransform;
            rectTransform.DOKill();

            rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
            _mainVisualMask.gameObject.SetActive(true);

            rectTransform
                .DOSizeDelta(new Vector2(_maxWidth, rectTransform.sizeDelta.y), _animationDuration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => onComplete ?.Invoke());
        }
    }
}
