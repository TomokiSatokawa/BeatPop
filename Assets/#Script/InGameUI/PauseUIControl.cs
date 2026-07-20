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

        public event Action OnReStart;
        public event Action OnStartCountDown;
        public event Action OnRetry;
        public event Action OnReturnTitle;

        private void Start()
        {
            InputManager.PauseButton.Where(b => b).Subscribe(_ => ChangeActive()).AddTo(this);

            _continueButton.onClick.AddListener(() => StartCountDown());
            _retryButton.onClick.AddListener(() => OnRetry?.Invoke());
            _titleButton.onClick.AddListener(() => OnReturnTitle?.Invoke());
        }

        public void ChangeActive()
        {
            if (_panelControl.IsActive)
            {
                StartCountDown();
            }
            else
            {
                //開く
                _panelControl.OnActive();
                GameManager.I.OnPause();
            }
        }
        public void StartCountDown()
        {
            _panelControl.OnHidden();
            OnStartCountDown.Invoke();
            _countDown.Play(OnReStart);
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(() => StartCountDown());
            _retryButton.onClick.RemoveListener(() => OnRetry?.Invoke());
            _titleButton.onClick.RemoveListener(() => OnReturnTitle?.Invoke());
        }
    }
}