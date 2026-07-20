using DG.Tweening;
using InGame.Score;
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
        [SerializeField] private RankData _rankData;
        [SerializeField] private RectTransform[] _rankLines;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private float _animationDuration = 0.3f;

        private int _currentScore;
        private Sequence _addScoreAnimation;

        private void Awake()
        {
            Initialize(); 
            InitializeRankLines();
        }

        private void Initialize()
        {
            _sliderImage.fillAmount = 0;
            _valueText.text = "0";
            _currentScore = 0;
        }

        private void InitializeRankLines()
        {
            RectTransform sliderRect = _sliderImage.rectTransform;

            float left = sliderRect.rect.xMin;
            float right = sliderRect.rect.xMax;

            for (int i = 0; i < _rankData.RankValue.Count && i < _rankLines.Length; i++)
            {
                float t = Mathf.Clamp01((float)_rankData.RankValue[i].Value);

                RectTransform rankLine = _rankLines[i];

                Vector2 pos = rankLine.anchoredPosition;
                pos.x = Mathf.Lerp(left, right, t);
                rankLine.anchoredPosition = pos;
            }
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