using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankUIControl : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Image _rankImage;
    [SerializeField] private RankData _rankData;

    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 0.15f;
    [SerializeField] private float _scaleUpDuration = 0.25f;
    [SerializeField] private float _scaleDownDuration = 0.15f;

    [SerializeField] private float _startScale = 0f;
    [SerializeField] private float _maxScale = 1.25f;
    [SerializeField] private float _endScale = 1f;

    [SerializeField] private Ease _scaleUpEase = Ease.OutBack;
    [SerializeField] private Ease _scaleDownEase = Ease.OutBounce;
    [SerializeField] private float _waitTime;

    private Tween _animation;

    public void OnAnimation(float scoreRate)
    {
        _rankImage.sprite = _rankData.GetRank(scoreRate);
        _rankImage.gameObject.SetActive(false);
        _animation?.Kill();

        _rankImage.transform.localScale = Vector3.one * _startScale;
        _rankImage.color = new Color(1f, 1f, 1f, 0f);

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(_waitTime);
        sequence.AppendCallback(() => _rankImage.gameObject.SetActive(true));
        sequence.Append(_rankImage.DOFade(1f, _fadeDuration));

        sequence.Join(
            _rankImage.transform
                .DOScale(_maxScale, _scaleUpDuration)
                .SetEase(_scaleUpEase)
        );

        sequence.Append(
            _rankImage.transform
                .DOScale(_endScale, _scaleDownDuration)
                .SetEase(_scaleDownEase)
        );

        _animation = sequence;
    }
}