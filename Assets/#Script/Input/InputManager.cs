using System;
using Common.PlaySystem;
using InGame;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Input
{
    /// <summary>
    /// InGameの入力を管理
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private TouchManager _touchManager;
        [SerializeField] private float _flickInterval;

        private static GameInputs _gameInputs;
        private static AutoPlayInput _autoPlayInput;
        private static TouchState[] _touchState;

        private readonly InputEvent _rightMainEvent = new();
        private readonly InputEvent _leftMainEvent = new();
        private readonly InputEvent _rightFlickEvent = new();
        private readonly InputEvent _leftFlickEvent = new();

        private readonly static ReactiveProperty<bool> _rightLane = new();
        private readonly static ReactiveProperty<bool> _leftLane = new();
        private readonly static ReactiveProperty<bool> _flickLeftLane = new();
        private readonly static ReactiveProperty<bool> _flickRightLane = new();
        private readonly static ReactiveProperty<bool> _pauseButton = new();
        private readonly static Subject<bool> _onRightFlick = new();
        private readonly static Subject<bool> _onLeftFlick = new();

        public static ReadOnlyReactiveProperty<bool> RightLane => _rightLane;
        public static ReadOnlyReactiveProperty<bool> LeftLane => _leftLane;
        public static ReadOnlyReactiveProperty<bool> FlickLeftLane => _flickLeftLane;
        public static ReadOnlyReactiveProperty<bool> FlickRightLane => _flickRightLane;
        public static ReadOnlyReactiveProperty<bool> PauseButton => _pauseButton;
        public static Observable<bool> OnRightFlick => _onRightFlick;
        public static Observable<bool> OnLeftFlick => _onLeftFlick;

        private Action _disableAction;

        public void Awake()
        {
            _gameInputs = new();

            RegisterAction(_gameInputs.Player.RightKey, OnRightKey);
            RegisterAction(_gameInputs.Player.LeftKey, OnLeftKey);
            RegisterAction(_gameInputs.Player.RightFlick, OnFlickRightKey);
            RegisterAction(_gameInputs.Player.LeftFlick, OnFlickLeftKey);
            RegisterAction(_gameInputs.UI.Pause, OnPauseKey);

            _touchState = new TouchState[4];

            RegisterTouchAction(_gameInputs.Player.Touch_0, 0);
            RegisterTouchAction(_gameInputs.Player.Touch_1, 1);
            RegisterTouchAction(_gameInputs.Player.Touch_2, 2);
            RegisterTouchAction(_gameInputs.Player.Touch_3, 3);

            _gameInputs.Enable();
        }

        public void Start()
        {
            InGameFileLoad.I.OnNodeFileLoaded.Skip(1).Subscribe(x =>
            {
                if (!SongPlayContext.I.IsAutoPlay)
                    return;

                _autoPlayInput = new(x.Nodes);

                _autoPlayInput.LeftMain += b => _leftLane.Value = b;
                _autoPlayInput.RightMain += b => _rightLane.Value = b;
                _autoPlayInput.RightFlick += b => _onRightFlick.OnNext(b);
                _autoPlayInput.LeftFlick += b => _onLeftFlick.OnNext(b);

                _gameInputs.Player.Disable();
            }).AddTo(this);
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
            bool performed = context.started || context.performed;
            _rightLane.Value = performed;

            _rightMainEvent.isDown = performed;
            _rightMainEvent.Time = (float)context.time;

            FlickCheck(_rightMainEvent, _rightFlickEvent, _onRightFlick);
        }

        private void OnLeftKey(InputAction.CallbackContext context)
        {
            bool performed = context.started || context.performed;
            _leftLane.Value = performed;

            _leftMainEvent.isDown = performed;
            _leftMainEvent.Time = (float)context.time;

            FlickCheck(_leftMainEvent, _leftFlickEvent, _onLeftFlick);
        }

        private void OnFlickRightKey(InputAction.CallbackContext context)
        {
            bool performed = context.started || context.performed;
            _flickRightLane.Value = performed;

            _rightFlickEvent.isDown = performed;
            _rightFlickEvent.Time = (float)context.time;

            FlickCheck(_rightMainEvent, _rightFlickEvent, _onRightFlick);
        }

        private void OnFlickLeftKey(InputAction.CallbackContext context)
        {
            bool performed = context.started || context.performed;
            _flickLeftLane.Value = performed;

            _leftFlickEvent.isDown = performed;
            _leftFlickEvent.Time = (float)context.time;

            FlickCheck(_leftMainEvent, _leftFlickEvent, _onLeftFlick);
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
                    if (touchState.phase == TouchPhase.Began)
                        break;

                    //フリック量
                    if (!_touchManager.IsFlick(touchState.startPosition, touchState.position))
                        break;
                    if (lane == 0)
                    {
                        _onLeftFlick.OnNext(false);
                    }
                    else
                    {
                        _onRightFlick.OnNext(false);
                    }
                    break;
                case TouchPhase.Moved:

                    //フリック量
                    if (!_touchManager.IsFlick(touchState.startPosition, touchState.position))
                        break;

                    lane = _touchManager.TapLane(touchState.startPosition);
                    if (lane == -1)
                        break;

                    if(lane == 0)
                    {
                        _onLeftFlick.OnNext(true);
                    }
                    else
                    {
                        _onRightFlick.OnNext(true);
                    }
                    break;
            }
        }

        private void FlickCheck(InputEvent main, InputEvent flick, Subject<bool> subject)
        {
            if (!flick.isDown) return;

            if (Mathf.Abs(main.Time - flick.Time) < _flickInterval)
            {
                subject.OnNext(main.isDown);
            }
        }

        private void OnDestroy()
        {
            _disableAction?.Invoke();
            _gameInputs.Disable();
        }

        private class InputEvent
        {
            public bool isDown;
            public float Time;
        }
    }
}