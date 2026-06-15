using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StartScreen
{
    public class StartScreenControl : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onClickAction;

        void Update()
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                _onClickAction?.Invoke();
                this.enabled = false;
            }
        }
    }
}