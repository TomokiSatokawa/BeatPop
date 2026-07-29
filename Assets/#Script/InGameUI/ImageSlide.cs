using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class ImageSlide : MonoBehaviour
    {
        [SerializeField] private Image _mainImage;
        [SerializeField] private Image _subImage;
        [SerializeField] private float _offScreen;
        [SerializeField] private float _animationDuration;

        private Sprite _currentSprite;
        public void SetImage(Sprite sprite)
        {
            _mainImage.sprite = sprite;
            _subImage.sprite = _currentSprite;

            _currentSprite = sprite;

        }

        public void ChangeImage(Sprite sprite)
        {
            SetImage(sprite);

            _mainImage.rectTransform.anchoredPosition = new Vector2(_offScreen, _mainImage.rectTransform.anchoredPosition.y);
            _subImage.rectTransform.anchoredPosition = new Vector2(0, _subImage.rectTransform.anchoredPosition.y);

            _mainImage.rectTransform.DOAnchorPosX(0, _animationDuration);
            _subImage.rectTransform.DOAnchorPosX(-_offScreen, _animationDuration);
        }
    }
}