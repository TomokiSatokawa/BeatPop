using UnityEngine;
using UnityEngine.Events;
namespace Common.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelControl : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UnityEvent _activeAction;
        [SerializeField] private UnityEvent _hiddenAction;

        public bool IsActive => _canvasGroup.alpha == 1.0f;
        private void Awake()
        {
            if (_canvasGroup == null) 
                _canvasGroup = GetComponent<CanvasGroup>();

            OnHidden();
        }

        public void OnActive()
        {
            _activeAction?.Invoke();
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.blocksRaycasts = true;
        }
        public void OnHidden()
        {
            _hiddenAction?.Invoke();
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}