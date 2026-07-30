using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace InGame.UI
{
    public class VideoSlide : MonoBehaviour
    {
        [SerializeField] private RawImage _mainImage;
        [SerializeField] private RawImage _subImage;

        [SerializeField] private VideoPlayer _mainPlayer;
        [SerializeField] private VideoPlayer _subPlayer;

        [SerializeField] private float _offScreen = 1920f;
        [SerializeField] private float _animationDuration = 0.5f;

        private Sequence _sequence;
        private VideoClip _currentClip;

        public void SetVideo(VideoClip clip)
        {
            _subPlayer.Stop();

            if (_currentClip != null)
            {
                _subPlayer.clip = _currentClip;
                _subPlayer.time = 0;
                _subPlayer.Play();
            }

            _mainPlayer.Stop();
            _mainPlayer.clip = clip;
            _mainPlayer.time = 0;
            _mainPlayer.Play();

            _currentClip = clip;
        }

        public bool IsCompleteAnimation()
        {
            return _sequence == null || !_sequence.IsActive() || _sequence.IsComplete();
        }

        public void ChangeVideoSlideFromRight(VideoClip clip)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            SetVideo(clip);

            _mainImage.rectTransform.anchoredPosition =
                new Vector2(_offScreen, _mainImage.rectTransform.anchoredPosition.y);

            _subImage.rectTransform.anchoredPosition =
                new Vector2(0, _subImage.rectTransform.anchoredPosition.y);

            _sequence.Append(
                _mainImage.rectTransform.DOAnchorPosX(0, _animationDuration));

            _sequence.Join(
                _subImage.rectTransform.DOAnchorPosX(-_offScreen, _animationDuration));
        }

        public void ChangeVideoSlideFromLeft(VideoClip clip)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            SetVideo(clip);

            _mainImage.rectTransform.anchoredPosition =
                new Vector2(-_offScreen, _mainImage.rectTransform.anchoredPosition.y);

            _subImage.rectTransform.anchoredPosition =
                new Vector2(0, _subImage.rectTransform.anchoredPosition.y);

            _sequence.Append(
                _mainImage.rectTransform.DOAnchorPosX(0, _animationDuration));

            _sequence.Join(
                _subImage.rectTransform.DOAnchorPosX(_offScreen, _animationDuration));
        }
    }
}