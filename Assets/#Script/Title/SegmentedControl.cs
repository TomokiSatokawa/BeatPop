using Common.UI;
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
        [SerializeField] protected TextButton[] _buttons;
        [SerializeField] protected Color _selectColor;
        [SerializeField] protected Color _unSelectColor;
        [SerializeField] protected Color _selectTextColor;
        [SerializeField] protected Color _unSelectTextColor;
        [SerializeField] protected Sprite _selectSprite;
        [SerializeField] protected Sprite _unSelectSprite;
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
            _buttons[index].Text.color = _unSelectTextColor;
        }

        public virtual void OnClick(int i)
        {
            //ButtonÇÃå©ÇΩñ⁄ïœçX
            _buttons[_currentIndex].image.color = _unSelectColor;
            _buttons[_currentIndex].image.sprite = _unSelectSprite;
            _buttons[_currentIndex].Text.color = _unSelectTextColor;

            _buttons[i].image.color = _selectColor;
            _buttons[i].image.sprite = _selectSprite;
            _buttons[i].Text.color = _selectTextColor;


            _currentIndex = i;
            _onValueChanged?.Invoke(i);
        }

        public virtual void SetButtonActive(int i, bool isActive)
        {
            _buttons[i].interactable = isActive;
        }
    }
}