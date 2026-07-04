using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class MissAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _maxAlpha = 0.5f;
        [SerializeField] private float _duration = 0.2f;

        private Sequence _sequence;

        private void Start()
        {
            _sequence = DOTween.Sequence()
                .Append(_image.DOFade(0, 0))
                .Append(_image.DOFade(_maxAlpha, _duration * 0.5f))
                .Append(_image.DOFade(0, _duration * 0.5f))
                .SetAutoKill(false)
                .Pause();
        }

        public void PlayAnimation()
        {
            _sequence.Restart();
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}