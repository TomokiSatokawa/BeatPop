using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager I;
        public static GameInputs GameInputs;

        private static ReactiveProperty<bool> _rightLane = new();
        public static ReadOnlyReactiveProperty<bool> RightLane => _rightLane;
        private static ReactiveProperty<bool> _leftLane = new();
        public static ReadOnlyReactiveProperty<bool> LeftLane => _leftLane; 
        private static ReactiveProperty<bool> _flickLeftLane = new();
        public static ReadOnlyReactiveProperty<bool> FlickLeftLane => _flickLeftLane;
        private static ReactiveProperty<bool> _flickRightLane = new();
        public static ReadOnlyReactiveProperty<bool> FlickRightLane => _flickRightLane;
        private void Awake()
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
            GameInputs.Enable();
        }

        public void OnRightKey(InputAction.CallbackContext context)
        {
            _rightLane.Value = context.started || context.performed;
        }

        public void OnLeftKey(InputAction.CallbackContext context)
        {
            _leftLane.Value = context.started || context.performed;
        }
        public void OnFlickRightKey(InputAction.CallbackContext context)
        {
            _flickRightLane.Value = context.started || context.performed;
        }
        public void OnFlickLeftKey(InputAction.CallbackContext context)
        {
            _flickLeftLane.Value = context.started || context.performed;
        }

    }

}