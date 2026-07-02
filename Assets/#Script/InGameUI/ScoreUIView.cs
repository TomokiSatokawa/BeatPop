using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIView : MonoBehaviour
{
    [SerializeField] private Image _sliderImage;
    [SerializeField] private RankData _rankData;
    [SerializeField] private RectTransform[] _rankLines;
    [SerializeField] private Image _rankImage;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private float _animationDuration = 0.3f;

    private int _maxScore = int.MaxValue;
    private int _currentScore;
    private Sprite _currentRank = null;

    private Sequence _addScoreAnimation;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _sliderImage.fillAmount = 0;
        _valueText.text = "0";
        _currentScore = 0;
    }

    public void SetData(int maxScore)
    {
        _maxScore = maxScore;

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

    public void UpdateScore(int score)
    {
        _addScoreAnimation?.Kill();

        int startScore = _currentScore;
        _currentScore = score;

        _addScoreAnimation = DOTween.Sequence();

        _addScoreAnimation.Join(
            _sliderImage.DOFillAmount((float)score / _maxScore, _animationDuration));

        _addScoreAnimation.Join(
            DOVirtual.Int(startScore, score, _animationDuration,
                x => _valueText.text = x.ToString()));

        RankAnimation(_rankData.GetRank((float)_currentScore / _maxScore));
    }

    private void RankAnimation(Sprite rank)
    {
        if (_currentRank == rank) return;
        _currentRank = rank;

        _rankImage.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_rankImage.rectTransform.DOScale(0f, 0.12f).SetEase(Ease.InBack));

        sequence.AppendCallback(() =>
        {
            _rankImage.sprite = rank;
        });

        sequence.Append(_rankImage.rectTransform.DOScale(1.2f, 0.18f).SetEase(Ease.OutBack));

        sequence.Append(_rankImage.rectTransform.DOScale(1f, 0.08f));
    }
}