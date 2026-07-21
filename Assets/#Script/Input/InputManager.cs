using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Input
{
    /// <summary>
    /// InGameÇÃì¸óÕÇä«óù
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private TouchManager _touchManager;

        private static GameInputs _gameInputs;
        private static TouchState[] _touchState;

        private readonly static ReactiveProperty<bool> _rightLane = new();
        private readonly static ReactiveProperty<bool> _leftLane = new();
        private readonly static ReactiveProperty<bool> _flickLeftLane = new();
        private readonly static ReactiveProperty<bool> _flickRightLane = new();
        private readonly static ReactiveProperty<bool> _pauseButton = new();
        private readonly static Subject<int> _onFlick = new();

        public static ReadOnlyReactiveProperty<bool> RightLane => _rightLane;
        public static ReadOnlyReactiveProperty<bool> LeftLane => _leftLane;
        public static ReadOnlyReactiveProperty<bool> FlickLeftLane => _flickLeftLane;
        public static ReadOnlyReactiveProperty<bool> FlickRightLane => _flickRightLane;
        public static ReadOnlyReactiveProperty<bool> PauseButton => _pauseButton;
        public static Observable<int> OnFlick => _onFlick;

        private Action _disableAction;

        public void Awake()
        {
            _gameInputs = new();

            RegisterAction(_gameInputs.Player.RightKey, OnRightKey);
            RegisterAction(_gameInputs.Player.LeftKey, OnLeftKey);
            RegisterAction(_gameInputs.Player.RightFlick, OnFlickRightKey);
            RegisterAction(_gameInputs.Player.LeftFlick, OnFlickLeftKey);
            RegisterAction(_gameInputs.Player.Pause, OnPauseKey);

            _touchState = new TouchState[4];

            RegisterTouchAction(_gameInputs.Player.Touch_0, 0);
            RegisterTouchAction(_gameInputs.Player.Touch_1, 1);
            RegisterTouchAction(_gameInputs.Player.Touch_2, 2);
            RegisterTouchAction(_gameInputs.Player.Touch_3, 3);

            _gameInputs.Enable();
        }

        private void RegisterAction(InputAction input, Action<InputAction.CallbackContext> action)
        {
            input.performed += action;
            input.canceled += action;

            _disableAction += () => input.performed -= action;
            _disableAction += () => input.canceled -= action;
        }

        private void RegisterTouchAction(InputAction input, int index)
        {
            Action<InputAction.CallbackContext> action = c => OnTouch(c, ref _touchState[index]);

            input.performed += action;
            input.canceled += action;

            _disableAction += () => input.performed -= action;
            _disableAction += () => input.canceled -= action;
        }

        public static void SetInputEnabled(bool enabled)
        {
            if (enabled)
            {
                _gameInputs.Player.RightKey.Enable();
                _gameInputs.Player.LeftKey.Enable();
                _gameInputs.Player.RightFlick.Enable();
                _gameInputs.Player.LeftFlick.Enable();
                _gameInputs.Player.Touch_0.Enable();
                _gameInputs.Player.Touch_1.Enable();
                _gameInputs.Player.Touch_2.Enable();
                _gameInputs.Player.Touch_3.Enable();
            }
            else
            {
                _gameInputs.Player.RightKey.Disable();
                _gameInputs.Player.LeftKey.Disable();
                _gameInputs.Player.RightFlick.Disable();
                _gameInputs.Player.LeftFlick.Disable();
                _gameInputs.Player.Touch_0.Disable();
                _gameInputs.Player.Touch_1.Disable();
                _gameInputs.Player.Touch_2.Disable();
                _gameInputs.Player.Touch_3.Disable();
            }
        }

        private void OnRightKey(InputAction.CallbackContext context)
        {
            _rightLane.Value = context.started || context.performed;
            if (_rightLane.Value)
            {
                FlickCheck(1);
            }
        }

        private void OnLeftKey(InputAction.CallbackContext context)
        {
            _leftLane.Value = context.started || context.performed;
            if (_leftLane.Value)
            {
                FlickCheck(0);
            }
        }

        private void OnFlickRightKey(InputAction.CallbackContext context)
        {
            _flickRightLane.Value = context.started || context.performed;
            if (_flickRightLane.Value)
            {
                FlickCheck(1);
            }
        }

        private void OnFlickLeftKey(InputAction.CallbackContext context)
        {
            _flickLeftLane.Value = context.started || context.performed;
            if (_flickLeftLane.Value)
            {
                FlickCheck(0);
            }
        }

        private void OnPauseKey(InputAction.CallbackContext context)
        {
            _pauseButton.Value = context.started || context.performed;
        }

        private void OnTouch(InputAction.CallbackContext context, ref TouchState touchState)
        {
            touchState = context.ReadValue<TouchState>();
            switch (touchState.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Ended:

                    int lane = _touchManager.TapLane(touchState.startPosition);

                    if (lane == 0)
                    {
                        _leftLane.OnNext(touchState.phase == TouchPhase.Began);
                    }
                    else if (lane == 1)
                    {
                        _rightLane.OnNext(touchState.phase == TouchPhase.Began);
                    }
                    break;
                case TouchPhase.Moved:

                    //ÉtÉäÉbÉNó 
                    if (!_touchManager.IsFlick(touchState.startPosition, touchState.position))
                        break;

                    lane = _touchManager.TapLane(touchState.startPosition);
                    if (lane == -1)
                        break;

                    _onFlick.OnNext(lane);
                    break;
            }
        }

        private void FlickCheck(int i)
        {
            if (i == 1 && RightLane.CurrentValue && FlickRightLane.CurrentValue)
            {
                _onFlick.OnNext(i);
            }

            if (i == 0 && LeftLane.CurrentValue && FlickLeftLane.CurrentValue)
            {
                _onFlick.OnNext(i);
            }
        }

        private void OnDestroy()
        {
            _disableAction?.Invoke();
            _gameInputs.Disable();
        }
    }
}