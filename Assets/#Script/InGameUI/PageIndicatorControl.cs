using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InGame.UI
{
    /// <summary>
    /// ページインディケーター
    /// </summary>
    public class PageIndicatorControl : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Sprite _visibleSprite;
        [SerializeField] private Sprite _hiddenSprite;
        [SerializeField] private int _startIndex;

        public UnityEvent<int> OnClickButton;

        private int _currentIndex = 0;
        private void Start()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                Button button = _buttons[index];
                button.image.sprite = _hiddenSprite;
                button.onClick.AddListener(() => OnClick(index));
            }

            ChangeIndex(_startIndex);
        }

        private void OnClick(int index)
        {
            ChangeIndex(index);
            OnClickButton?.Invoke(index);
        }

        public void ChangeIndex(int index)
        {

            if(index < 0  || index >= _buttons.Length)
            {
                return;
            }

            _buttons[_currentIndex ].image.sprite = _hiddenSprite;
            _currentIndex = index;
            _buttons[_currentIndex].image.sprite = _visibleSprite;
        }

    }
}