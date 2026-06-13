using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClearAnimation : MonoBehaviour
{
    [SerializeField] private Image _mainVisualMask;
    [SerializeField] private float _maxWith;
    [SerializeField] private float _animationDuration;
    private void Start()
    {
        _mainVisualMask.gameObject.SetActive(false);
    }
    public void OnAnimation()
    {
        _mainVisualMask.rectTransform.sizeDelta = new Vector2(0, _mainVisualMask.rectTransform.sizeDelta.y);
        _mainVisualMask.gameObject.SetActive(true);
        _mainVisualMask.rectTransform
            .DOSizeDelta(new Vector2(_maxWith, _mainVisualMask.rectTransform.sizeDelta.y),_animationDuration)
            .SetEase(Ease.InOutCubic);
    }
}
