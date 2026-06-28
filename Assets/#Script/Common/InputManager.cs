using System;
using R3;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private static GameInputs GameInputs;

        private static ReactiveProperty<bool> _rightLane = new();
        public static ReadOnlyReactiveProperty<bool> RightLane => _rightLane;
        private static ReactiveProperty<bool> _leftLane = new();
        public static ReadOnlyReactiveProperty<bool> LeftLane => _leftLane; 
        private static ReactiveProperty<bool> _flickLeftLane = new();
        public static ReadOnlyReactiveProperty<bool> FlickLeftLane => _flickLeftLane;
        private static ReactiveProperty<bool> _flickRightLane = new();
        public static ReadOnlyReactiveProperty<bool> FlickRightLane => _flickRightLane;

        private static ReactiveProperty<bool> _pauseButton = new();
        public static ReadOnlyReactiveProperty<bool> PauseButton => _pauseButton;

        private static Subject<int> _onFlick = new();
        public static Observable<int> OnFlick => _onFlick;

        public override void Awake()
        {
            GameInputs = new();

            GameInputs.Player.RightKey.performed += OnRightKey;
            GameInputs.Player.RightKey.canceled += OnRightKey;

            GameInputs.Player.LeftKey.performed += OnLeftKey;
            GameInputs.Player.LeftKey.canceled += OnLeftKey;

            GameInputs.Player.RightFlick.performed += OnFlickRightKey;
            GameInputs.Player.RightFlick.canceled += OnFlickRightKey;

            GameInputs.Player.LeftFlick.performed += OnFlickLeftKey;
            GameInputs.Player.LeftFlick.canceled += OnFlickLeftKey;

            GameInputs.Player.Pause.performed += OnPauseKey;
            GameInputs.Player.Pause.canceled += OnPauseKey;

            GameInputs.Player.Tap.performed += OnTap;
            GameInputs.Player.Tap.canceled += OnTap;

            GameInputs.Enable();
        }

        public void OnRightKey(InputAction.CallbackContext context)
        {
            _rightLane.Value = context.started || context.performed;
            if(_rightLane.Value)
            {
                FlickCheck(1);
            }
        }

        public void OnLeftKey(InputAction.CallbackContext context)
        {
            _leftLane.Value = context.started || context.performed;
            if (_leftLane.Value)
            {
                FlickCheck(0);
            }
        }
        public void OnFlickRightKey(InputAction.CallbackContext context)
        {
            _flickRightLane.Value = context.started || context.performed;
            if (_flickRightLane.Value)
            {
                FlickCheck(1);
            }
        }
        public void OnFlickLeftKey(InputAction.CallbackContext context)
        {
            _flickLeftLane.Value = context.started || context.performed;
            if (_flickLeftLane.Value)
            {
                FlickCheck(0);
            }
        }

        public void OnPauseKey(InputAction.CallbackContext context)
        {
            _pauseButton.Value = context.started || context.performed;
        }

        public void OnTap(InputAction.CallbackContext context)
        {

        }

        public static void SetInputEnabled(bool enabled)
        {
            if(enabled)
            {
                GameInputs.Player.RightKey.Enable();
                GameInputs.Player.LeftKey.Enable();
                GameInputs.Player.RightFlick.Enable();
                GameInputs.Player.LeftFlick.Enable();
            }
            else
            {
                GameInputs.Player.RightKey.Disable();
                GameInputs.Player.LeftKey.Disable();
                GameInputs.Player.RightFlick.Disable();
                GameInputs.Player.LeftFlick.Disable();
            }
        }

        private void FlickCheck(int i)
        {
            if(i == 1 && RightLane.CurrentValue && FlickRightLane.CurrentValue)
            {
                _onFlick.OnNext(i);
            }

            if(i == 0 && LeftLane.CurrentValue && FlickLeftLane.CurrentValue)
            {
                _onFlick.OnNext(i);
            }
        }

        private void SetInputAction(GameInputs.PlayerActions input,Action<InputAction.CallbackContext> action)
        {
            //input.Pause
        }
    }
}