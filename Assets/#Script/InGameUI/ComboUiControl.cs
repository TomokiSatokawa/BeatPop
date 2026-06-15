using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboUIControl : MonoBehaviour
{
    [SerializeField] private Transform _paretObject;
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _afterText;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _minSize;
    [SerializeField] private float _afterMaxSize;
    [SerializeField] private float _animationDuration;

    private Sequence _sequence;

    public float _defaultSize;
    public void Start()
    {
        _defaultSize = _paretObject.transform.localScale.x;
    }
    public void UpdateCombo(int count)
    {
        _paretObject.gameObject.SetActive(true);
        _sequence.Kill(true);

        _sequence = DOTween.Sequence();
        _paretObject.transform.localScale = Vector3.one * _minSize;
        _afterText.transform.localScale = Vector3.one * _minSize;
        _afterText.color = Color.black;
        _afterText.text = count.ToString();
        _comboText.text = count.ToString();
        _sequence.Append(_paretObject.DOScale(_maxSize, _animationDuration * 0.7f));

        _sequence.Join(_afterText.transform.DOScale(_afterMaxSize, _animationDuration/2));
        _sequence.Join(_afterText.DOFade(0, _animationDuration));

        _sequence.Append(_paretObject.DOScale(_defaultSize, _animationDuration * 0.3f));

        _sequence.Play();
    }
    public void HiddenUI()
    {
        _paretObject.gameObject.SetActive(false);
    }
}
