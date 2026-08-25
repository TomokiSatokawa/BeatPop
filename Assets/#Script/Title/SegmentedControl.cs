using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title
{
    /// <summary>
    /// SegmentedUI
    /// </summary>
    public class SegmentedControl : MonoBehaviour
    {
        [SerializeField] protected Button[] _buttons;
        [SerializeField] protected Color _selectColor;
        [SerializeField] protected Color _unSelectColor;
        [SerializeField] protected int _startIndex;
        [SerializeField] protected UnityEvent<int> _onValueChanged;
        protected int _currentIndex = -1;
        public int CurrentIndex => _currentIndex;
        public int StartIndex => _startIndex;

        private void Start()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                InitializeButton(i);
            }

            if (_currentIndex == -1)
            {
                _currentIndex = _startIndex;
                OnClick(_currentIndex);
            }
        }

        protected  virtual void InitializeButton(int index)
        {
            _buttons[index].onClick.AddListener(() => OnClick(index));
            _buttons[index].image.color = _unSelectColor;
        }

        public virtual void OnClick(int i)
        {
            //ButtonÇÃêFïœçX
            _buttons[_currentIndex].image.color = _unSelectColor;
            _buttons[i].image.color = _selectColor;

            _currentIndex = i;
            _onValueChanged?.Invoke(i);
        }

        public virtual void SetButtonActive(int i, bool isActive)
        {
            _buttons[i].interactable = isActive;
        }
    }
}