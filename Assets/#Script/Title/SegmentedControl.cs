using Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Title
{
    public class SegmentedControl : MonoBehaviour
    {
        [SerializeField] private DifficultyColor _buttonColor;
        [SerializeField] private DifficultyButtonControl[] _buttons;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _unSelectColor;
        [SerializeField] private int _startIndex;
        [SerializeField] private UnityEvent<int> _onValueChanged;
        private int _currentIndex = -1;
        public int CurrentIndex => _currentIndex;
        public int StartIndex => _startIndex;
        private void Start()
        {
            for(int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[i].MainButton.onClick.AddListener(() => OnClick(index));
                Color mainColor = _buttonColor.GetDifficultyColor((Difficulty)i);
                _buttons[i].SetColor(mainColor, _selectColor, _unSelectColor);
            }
            if (_currentIndex == -1)
            {
                _currentIndex = _startIndex;
                OnClick(_currentIndex);
            }
        }

        public void OnClick(int i)
        {
            //ButtonÇÃêFïœçX
            _buttons[_currentIndex].IsSelect(false);
            _buttons[i].IsSelect(true);

            _currentIndex = i;
            _onValueChanged?.Invoke(i);
        }

        public void SetButtonActive(int i, bool isActive)
        {
            _buttons[i].MainButton.interactable = isActive;
        }

        public void SetButtonLevel(int i, int level)
        {
            _buttons[i].SetLevel(level);
        }
    }
}
