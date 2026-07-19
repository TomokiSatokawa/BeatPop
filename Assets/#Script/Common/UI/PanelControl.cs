using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI
{
    /// <summary>
    /// UIWindow‚Ì•\Ž¦/”ñ•\Ž¦
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelControl : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UnityEvent _activeAction = new();
        [SerializeField] private UnityEvent _hiddenAction = new();

        private Tween _fadeAnimation;
        public bool IsActive { get; private set; }
        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            SetVisible(false,0);
        }

        public void OnActive(float duration = 0)
        {
            _activeAction?.Invoke();

            SetVisible(true, duration);
        }
        public void OnHidden(float duration = 0)
        {
            _hiddenAction?.Invoke();

           SetVisible(false,duration);
        }
        
        private void SetVisible(bool visible,float fadeDuration)
        {
            float alpha = visible ? 1f : 0f;

            if (fadeDuration > 0f)
            {
                Fade(alpha, fadeDuration);
            }
            else
            {
                _canvasGroup.alpha = alpha;
            }

            _canvasGroup.blocksRaycasts = visible;
            IsActive = visible;
        }

        private void Fade(float amount, float duration)
        {
            _fadeAnimation?.Kill();
            _fadeAnimation = _canvasGroup.DOFade(amount, duration);
        }

        private void OnDestroy()
{
    _fadeAnimation?.Kill();
}
    }
}