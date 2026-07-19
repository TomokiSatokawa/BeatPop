using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Input
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        public TextMeshProUGUI _text;
        [SerializeField] private TouchManager _touchManager;
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

        private static TouchState[] _touchState;

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

            _touchState = new TouchState[4];

            GameInputs.Player.Touch_0.performed += c => OnTouch(c, ref _touchState[0]);
            GameInputs.Player.Touch_1.performed += c => OnTouch(c, ref _touchState[1]);
            GameInputs.Player.Touch_2.performed += c => OnTouch(c, ref _touchState[2]);
            GameInputs.Player.Touch_3.performed += c => OnTouch(c, ref _touchState[3]);

            GameInputs.Enable();
        }

        public void OnRightKey(InputAction.CallbackContext context)
        {
            _rightLane.Value = context.started || context.performed;
            if (_rightLane.Value)
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

        public void OnTouch(InputAction.CallbackContext context, ref TouchState touchState)
        {
            touchState = context.ReadValue<TouchState>();
            switch (touchState.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Ended:

                    _text.text = touchState.startPosition.ToString();
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

                    //ƒtƒŠƒbƒN—Ê
                    if (!_touchManager.IsFlick(touchState.startPosition, touchState.position))
                        break;

                    lane = _touchManager.TapLane(touchState.startPosition);
                    if (lane == -1)
                        break;

                    _onFlick.OnNext(lane);
                    break;
            }
        }

        public static void SetInputEnabled(bool enabled)
        {
            if (enabled)
            {
                GameInputs.Player.RightKey.Enable();
                GameInputs.Player.LeftKey.Enable();
                GameInputs.Player.RightFlick.Enable();
                GameInputs.Player.LeftFlick.Enable();
                GameInputs.Player.Touch_0.Enable();
                GameInputs.Player.Touch_1.Enable();
                GameInputs.Player.Touch_2.Enable();
                GameInputs.Player.Touch_3.Enable();
            }
            else
            {
                GameInputs.Player.RightKey.Disable();
                GameInputs.Player.LeftKey.Disable();
                GameInputs.Player.RightFlick.Disable();
                GameInputs.Player.LeftFlick.Disable();
                GameInputs.Player.Touch_0.Disable();
                GameInputs.Player.Touch_1.Disable();
                GameInputs.Player.Touch_2.Disable();
                GameInputs.Player.Touch_3.Disable();
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
        private void OnDisable()
        {
            GameInputs.Disable();
        }
    }
}