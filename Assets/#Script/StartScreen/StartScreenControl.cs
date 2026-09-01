using System.Threading;
using Common;
using Common.UI;
using Cysharp.Threading.Tasks;
using Sound;
using Title.Custom;
using Title.PlayerData;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StartScreen
{
    public class StartScreenControl : MonoBehaviour
    {
        [SerializeField] private CustomDataLoader _manifestLoader;
        [SerializeField] private PlayerDataLoader _playerDataLoader;
        [SerializeField] private FadeImageControl _fadeImageControl;
        [SerializeField] private SceneTransition _sceneTransition;
        [SerializeField] private float _bgmFadeDuration;
        [SerializeField] private GameObject _loadText;
        [SerializeField] private AudioClip _titleBGM;

        private bool _isLoading;
        private void Start()
        {
#if UNITY_STANDALONE_WIN
            SetWindowResolution(1170, 2532);
#endif
            _isLoading = false;
            SoundManager.BGM.PlayBGM(_titleBGM);
            _loadText.gameObject.SetActive(false);
        }
        private void SetWindowResolution(int targetWidth, int targetHeight)
        {
            Resolution maxResolution = Screen.currentResolution;

            float scale = Mathf.Min(
                (float)maxResolution.width / targetWidth,
                (float)maxResolution.height / targetHeight,
                1f // Œ³‚ÌƒTƒCƒY‚æ‚è‘å‚«‚­‚µ‚È‚¢
            );

            int width = Mathf.RoundToInt(targetWidth * scale);
            int height = Mathf.RoundToInt(targetHeight * scale);

            Screen.SetResolution(
                width,
                height,
                FullScreenMode.Windowed
            );
        }
        private async UniTask LoadSaveData()
        {
            _loadText.gameObject.SetActive(true);
            await _manifestLoader.LoadManifest();
            await _playerDataLoader.LoadData();
            _loadText.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (_isLoading) return;
            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame))
            {
                _isLoading = true;
                StartGame();
            }
        }
        private async void StartGame()
        {
            await LoadSaveData();
            SoundManager.BGM.VolumeFade(0, _bgmFadeDuration);

            UniTask fadeScreen = _fadeImageControl.FadeOut(FadeType.White);
            UniTask fadeBGM = UniTask.WaitForSeconds(_bgmFadeDuration);
            await UniTask.WhenAll(fadeScreen, fadeBGM);
            await _sceneTransition.LoadSceneAsync("Title", new CancellationTokenSource().Token);
        }
    }
}