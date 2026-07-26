using DG.Tweening;
using InGame.Score;
using Title.SongSelect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    /// <summary>
    /// InGame中のスコア表示
    /// </summary>
    public class ScoreUIView : MonoBehaviour
    {
        [SerializeField] private Image _sliderImage;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private float _animationDuration = 0.3f;

        private int _currentScore;
        private Sequence _addScoreAnimation;

        private void Awake()
        {
            Initialize(); 
        }

        private void Initialize()
        {
            _sliderImage.fillAmount = 0;
            _valueText.text = "0";
            _currentScore = 0;
        }


        public void UpdateScore(int score,float scoreRatio)
        {
            _addScoreAnimation?.Kill();

            int startScore = _currentScore;
            _currentScore = score;

            _addScoreAnimation = DOTween.Sequence();

            _addScoreAnimation.Join(
                _sliderImage.DOFillAmount(scoreRatio, _animationDuration));

            _addScoreAnimation.Join(
                DOVirtual.Int(startScore, score, _animationDuration,
                    x => _valueText.text = x.ToString()));

        }

        private void OnDestroy()
        {
            _addScoreAnimation?.Kill();
        }
    }
}