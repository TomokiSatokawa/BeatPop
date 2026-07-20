using DG.Tweening;
using TMPro;
using UnityEngine;
namespace InGame.UI
{
    /// <summary>
    /// ƒRƒ“ƒ{UI‚ÌView
    /// </summary>
    public class ComboUIControl : MonoBehaviour
    {
        [SerializeField] private Transform _parentObject;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private TextMeshProUGUI _afterText;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _minSize;
        [SerializeField] private float _afterMaxSize;
        [SerializeField] private float _animationDuration;

        private Sequence _sequence;
        private float _defaultSize;
        private void Start()
        {
            _defaultSize = _parentObject.transform.localScale.x;
            CreateAnimation();
        }

        private void CreateAnimation()
        {
            _sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Pause();

            _sequence.AppendCallback(() =>
            {
                _parentObject.gameObject.SetActive(true);
                _parentObject.localScale = Vector3.one * _minSize;
                _afterText.transform.localScale = Vector3.one * _minSize;
                _afterText.color = Color.black;

            });

            _sequence.Append(_parentObject.DOScale(_maxSize, _animationDuration * 0.7f).From(_minSize));
            _sequence.Join(_afterText.transform.DOScale(_afterMaxSize, _animationDuration / 2).From(_minSize));
            _sequence.Join(_afterText.DOFade(0, _animationDuration));

            _sequence.Append(_parentObject.DOScale(_defaultSize, _animationDuration * 0.3f));
            _sequence.ForceInit();

        }

        public void UpdateCombo(int count)
        {
            _comboText.text = count.ToString();
            _afterText.text = count.ToString();

            _sequence.Restart(true);
        }
        public void HiddenUI()
        {
            _parentObject.gameObject.SetActive(false);
        }
    }
}