using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class ClearAnimation : MonoBehaviour
{
    [SerializeField] private Image _mainVisualMask;
    [SerializeField] private SceneTransition _sceneLoad;
    [SerializeField] private float _maxWith;
    [SerializeField] private float _animationDuration;
    private void Start()
    {
        _mainVisualMask.gameObject.SetActive(false);
    }
    public void PlayClearAnimation()
    {
        RectTransform rectTransform = _mainVisualMask.rectTransform;
        rectTransform.DOKill();

        _sceneLoad.ChangeScene(3);
        rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
        _mainVisualMask.gameObject.SetActive(true);
        rectTransform
            .DOSizeDelta(new Vector2(_maxWith, rectTransform.sizeDelta.y), _animationDuration)
            .SetEase(Ease.InOutCubic);
            //TODO: ‰¼
            //.OnComplete(() => );
    }
}
