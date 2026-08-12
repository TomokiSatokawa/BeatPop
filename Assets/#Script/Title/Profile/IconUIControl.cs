using System;
using UnityEngine;
using UnityEngine.UI;

namespace Title.Profile
{
    /// <summary>
    /// アイコンUIのコントロール
    /// </summary>
    public class IconUIControl : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _selectButton;
        public RectTransform Rect => _rectTransform;

        private int _iconId = -1;
        private Action<int> _onSelect;
        
        private void Start()
        {
            _selectButton.onClick.RemoveAllListeners();
            _selectButton.onClick.AddListener(() => _onSelect?.Invoke(_iconId));
        }

        public void SetIcon(Sprite icon,int id)
        {
            _iconImage.sprite = icon;
            _iconId = id;
        }

        public void SetSelectAction(Action<int> onSelect)
        { 
            _onSelect = onSelect;
        }
    }
}