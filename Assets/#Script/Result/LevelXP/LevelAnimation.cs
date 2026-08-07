using DG.Tweening;
using Title.PlayerData;
using TMPro;
using UnityEngine;

namespace Result.UI
{
    public class LevelAnimation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("Scale")]
        [SerializeField] private float _startScale = 0.4f;
        [SerializeField] private float _overshootScale = 1.35f;
        [SerializeField] private float _endScale = 1f;

        [Header("Duration")]
        [SerializeField] private float _popDuration = 0.22f;
        [SerializeField] private float _settleDuration = 0.18f;

        [SerializeField] private Ease _popEase = Ease.OutExpo;
        [SerializeField] private Ease _settleEase = Ease.OutBack;

        private Sequence _sequence;

        private void Awake()
        {
            _levelText.text = PlayerDataLoader.Info.Level.ToString();
            _sequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Pause();

            _sequence.AppendCallback(() =>
            {
                _levelText.transform.localScale = Vector3.one * _startScale;

                var color = _levelText.color;
                color.a = 0f;
                _levelText.color = color;
            });

            _sequence.Join(_levelText.DOFade(1f, _popDuration * 0.7f));

            _sequence.Append(
                _levelText.transform
                    .DOScale(_overshootScale, _popDuration)
                    .SetEase(_popEase));

            _sequence.Append(
                _levelText.transform
                    .DOScale(_endScale, _settleDuration)
                    .SetEase(_settleEase));
        }

        public void Play(int level)
        {
            _sequence.Rewind();

            _levelText.text = level.ToString();

            _sequence.Restart();
        }
    }
}