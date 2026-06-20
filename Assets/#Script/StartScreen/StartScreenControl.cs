using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StartScreen
{
    public class StartScreenControl : MonoBehaviour
    {
        [SerializeField] private CustomManifestLoader _manifestLoader;
        [SerializeField] private UnityEvent _onClickAction;

        private bool _isLoaded;
        private void Start()
        {
            _isLoaded = false;
            Initialize();
        }
        private async void Initialize()
        {
            var ct = this.GetCancellationTokenOnDestroy();
            await _manifestLoader.LoadManifest(ct);

            _isLoaded = true;
        }
        void Update()
        {
            if (!_isLoaded) return;
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                _onClickAction?.Invoke();
                this.enabled = false;
            }
        }
    }
}