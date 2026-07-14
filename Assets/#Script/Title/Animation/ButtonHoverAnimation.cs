using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _hoverScale = 1.1f;
    [SerializeField] private float _duration = 0.15f;
    [SerializeField] private Ease _ease = Ease.OutBack;

    private Vector3 _defaultScale;
    private Tween _tween;

    private void Awake()
    {
        _defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = transform.DOScale(_defaultScale * _hoverScale, _duration)
            .SetEase(_ease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = transform.DOScale(_defaultScale, _duration)
            .SetEase(Ease.OutQuad);
    }

    private void OnDisable()
    {
        _tween?.Kill();
        transform.localScale = _defaultScale;
    }
}