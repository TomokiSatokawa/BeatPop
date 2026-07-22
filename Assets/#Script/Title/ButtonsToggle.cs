using UnityEngine;
using UnityEngine.UI;

namespace Title.Common
{
    public class ButtonsToggle : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private bool[] _isOn;

        [SerializeField] private Color _onColor;
        [SerializeField] private Color _offColor;
        public bool[] IsOn => _isOn;
        void Start()
        {
            if (_isOn.Length != _buttons.Length)
            {
                _isOn = new bool[_buttons.Length];
            }

            for (int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[index].onClick.AddListener(() =>
                {
                    OnClick(index);
                });

                _buttons[i].image.color = _isOn[i] ? _onColor : _offColor;
            }
        }

        public void OnClick(int buttonNum)
        {
            _isOn[buttonNum] = !_isOn[buttonNum];
            _buttons[buttonNum].image.color = _isOn[buttonNum] ? _onColor : _offColor;
        }
    }
}