using DG.Tweening;
using TMPro;
using UnityEngine;

public class JudgeUIControl : PoolObject
{
    [SerializeField] private float _moveAmount;
    [SerializeField] private float _duration;
    [SerializeField] private TextMeshProUGUI _text;
    public TextMeshProUGUI Text => _text;

    private Sequence _sequence;
    private void Awake()
    {
        _sequence = DOTween.Sequence()
            .Append(_text.rectTransform.DOAnchorPosY(_moveAmount, _duration).SetRelative())
            .Append(_text.DOFade(0, _duration / 2))
            .AppendCallback(() => Release())
            .SetAutoKill(false)
            .Pause();
    }

    private void OnEnable()
    {
        _sequence.Restart(true);
    }
}
