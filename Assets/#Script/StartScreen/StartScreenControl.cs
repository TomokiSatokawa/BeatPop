using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StartScreen
{
    public class StartScreenControl : MonoBehaviour
    {
        [SerializeField] private CustomDataLoader _manifestLoader;
        [SerializeField] private UnityEvent _onClickAction;
        [SerializeField] private GameObject _loadText;

        private bool _isLoaded;
        private void Start()
        {
            _isLoaded = false;
            Initialize();
        }
        private async void Initialize()
        {
            _loadText.gameObject.SetActive(true);
            var ct = this.GetCancellationTokenOnDestroy();
            await _manifestLoader.LoadManifest(ct);

            _isLoaded = true;
            _loadText.gameObject.SetActive(false);
        }
        void Update()
        {
            if (!_isLoaded) return;
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame))
            {
                _onClickAction?.Invoke();
                this.enabled = false;
            }
        }
    }
}