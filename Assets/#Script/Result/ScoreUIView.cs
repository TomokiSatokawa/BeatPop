using DG.Tweening;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Result.UI
{
    /// <summary>
    /// ScoreUI‚ÌView
    /// </summary>
    public class ScoreUIView : MonoBehaviour
    {
        [SerializeField] private Image _scoreSlider;
        [SerializeField] private TextMeshProUGUI _scoreValue;
        [SerializeField] private float _animationDuration;

        private Sequence _sequence;

        public void OnAnimation(int score, int maxScore)
        {
            float fillAmount = maxScore <= 0 ? 0f : (float)score / maxScore;
            _scoreSlider.fillAmount = 0;
            SoundManager.SE.PlaySE(SESoundType.ScoreCount);

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_scoreSlider.DOFillAmount(fillAmount, _animationDuration));
            _sequence.Join(DOVirtual.Int(0, score, _animationDuration,
                x => _scoreValue.text = x.ToString()));

            _sequence.Play();
        }
    }
}