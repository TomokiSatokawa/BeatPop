using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Title
{
    public class SegmentedControl : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _unSelectColor;
        [SerializeField] private int _startIndex;
        [SerializeField] private UnityEvent<int> _onValueChanged;
        private int _currentIndex = -1;
        public int CurrentIndex => _currentIndex;
        private void Start()
        {
            for(int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[i].onClick.AddListener(() => OnClick(index));
                _buttons[i].image.color= _unSelectColor;
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
            _buttons[_currentIndex].image.color = _unSelectColor;
            _buttons[i].image.color = _selectColor;

            _currentIndex = i;
            _onValueChanged?.Invoke(i);
        }
        public void SetButtonActive(int i, bool isActive)
        {
            _buttons[i].interactable = isActive;
        }
    }
}
