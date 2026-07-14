using DG.Tweening;
using TMPro;
using UnityEngine;

public class JudgeUIControl : PoolObject
{
    [Header("Animation")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _popScale = 1.2f;
    [SerializeField] private float _popDuration = 0.08f;
    [SerializeField] private float _returnDuration = 0.12f;
    [SerializeField] private float _fadeDelay = 0.25f;
    [SerializeField] private float _fadeDuration = 0.25f;
    [SerializeField] private float _defaultSize = 1f;

    [SerializeField] private TextMeshProUGUI _text;
    public TextMeshProUGUI Text => _text;

    private Sequence _sequence;
    private void Awake()
    {
        _sequence = DOTween.Sequence()
            .Append(_text.rectTransform.DOScale(_popScale, _popDuration).SetEase(Ease.OutBack))
            .Append(_text.rectTransform.DOScale(_defaultSize, _returnDuration).SetEase(Ease.OutQuad))
            .Join(_text.DOFade(0f, _fadeDuration).SetDelay(_fadeDelay))
            .SetAutoKill(false)
            .Pause();
    }

    private void OnEnable()
    {
        _text.rectTransform.localScale = Vector3.one;
        _text.alpha = 1f;
    }

    public void OnPlay()
    {
        _sequence.Restart();
    }
}