using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Result.UI
{
    public class ScoreUIControl : MonoBehaviour
    {
        [SerializeField] private Image _scoreSlider;
        [SerializeField] private TextMeshProUGUI _scoreValue;

        [SerializeField] private float _animationDuration;
        public void OnAnimation(int score, int maxScore)
        {
            Debug.Log(score + " " + maxScore);
            float fillAmount = maxScore <= 0 ? 0f : (float)score / maxScore;

            _scoreSlider.DOFillAmount(fillAmount, _animationDuration);

            DOVirtual.Int(0, score, _animationDuration,
                x => _scoreValue.text = x.ToString());
        }
    }
}