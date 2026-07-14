using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
namespace Common.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelControl : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UnityEvent _activeAction = new();
        [SerializeField] private UnityEvent _hiddenAction = new();

        private Tween _fadeAniamtion;
        public bool IsActive => _canvasGroup.alpha == 1.0f;
        private void Awake()
        {
            if (_canvasGroup == null) 
                _canvasGroup = GetComponent<CanvasGroup>();

            OnHidden();
        }

        public void OnActive(float duration = 0)
        {
            _activeAction?.Invoke();
            _canvasGroup.alpha = 0f;
            Fade(1, duration);
            _canvasGroup.blocksRaycasts = true;
        }
        public void OnHidden(float duration = 0)
        {
            _hiddenAction?.Invoke();
            _canvasGroup.alpha = 1f;
            Fade(0, duration);
            _canvasGroup.blocksRaycasts = false;
        }

        private void Fade(float amount,float duration)
        {
            _fadeAniamtion?.Kill();
            _fadeAniamtion = _canvasGroup.DOFade(amount,duration);
        }
    }
}