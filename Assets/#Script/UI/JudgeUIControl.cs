using DG.Tweening;
using TMPro;
using UnityEngine;

public class JudgeUIControl : MonoBehaviour
{
    [SerializeField] private float _moveAmount;
    [SerializeField] private float _duration;
    [SerializeField] private TextMeshProUGUI _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.rectTransform.DOAnchorPosY(_text.rectTransform.anchoredPosition.y + _moveAmount, _duration);
        _text.DOFade(0, _duration / 2);
        Destroy(this.gameObject, _duration + 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
