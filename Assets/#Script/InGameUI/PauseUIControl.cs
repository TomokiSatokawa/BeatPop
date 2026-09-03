using System;
using Common.UI;
using Input;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    /// <summary>
    /// ポーズUI
    /// </summary>
    public class PauseUIControl : MonoBehaviour
    {
        [SerializeField] private CountDownUI _countDown;
        [SerializeField] private PanelControl _panelControl;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _titleButton;

        private readonly Subject<Unit> _onRetry = new(); 
        private readonly Subject<Unit> _onReStart = new();
        private readonly Subject<Unit> _onStartCountDown = new();
        private readonly Subject<Unit> _onReturnTitle = new();

        public Observable<Unit> OnRetry => _onRetry;
        public Observable<Unit> OnReStart => _onReStart;
        public Observable<Unit> OnStartCountDown => _onStartCountDown;
        public Observable<Unit> OnReturnTitle => _onReturnTitle;

        private void Start()
        {
            InputManager.PauseButton.Where(b => b).Subscribe(_ => ChangeActive()).AddTo(this);

            _continueButton.onClick.AddListener(() => StartCountDown());
            _retryButton.onClick.AddListener(() => _onRetry.OnNext(Unit.Default));
            _titleButton.onClick.AddListener(() => _onReturnTitle.OnNext(Unit.Default));
        }

        public void ChangeActive()
        {
            //ポーズ時間外はポーズ不可
            if (StageTimeController.StageTime < 0 || !StageTimeController.I.IsPlaying.CurrentValue) return;

            if (_panelControl.IsActive)
            {
                StartCountDown();
            }
            else
            {
                //開く
                OnOpen();
            }
        }

        public void OnOpen()
        {
            _panelControl.OnActive();
            _countDown.Stop();
            GameManager.I.Pause();
        }

        public void StartCountDown()
        {
            _panelControl.OnHidden();
            _onStartCountDown.OnNext(Unit.Default);
            _countDown.Play(() => _onReStart.OnNext(Unit.Default));
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(() => StartCountDown());
            _retryButton.onClick.RemoveListener(() => _onRetry.OnNext(Unit.Default));
            _titleButton.onClick.RemoveListener(() => _onReturnTitle.OnNext(Unit.Default));
        }
    }
}