using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// アイテム追加UI
    /// </summary>
    public class ItemAddView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _totalCount;
        [SerializeField] private TextMeshProUGUI _addCount;
        [SerializeField] private int _digitCount;
        [SerializeField] private float _delay;
        [Header("Add Count")]
        [SerializeField] private float _addFadeDuration = 0.2f;
        [SerializeField] private Ease _addFadeInEase = Ease.OutCubic;
        [SerializeField] private Ease _addFadeOutEase = Ease.InCubic;

        [SerializeField] private float _addScaleDuration = 0.25f;
        [SerializeField] private float _addScale = 1.2f;
        [SerializeField] private Ease _addScaleEase = Ease.OutBack;

        [SerializeField] private float _addMoveDuration = 0.6f;
        [SerializeField] private float _addMoveDistance = 50f;
        [SerializeField] private Ease _addMoveEase = Ease.OutCubic;

        [Header("Total Count")]
        [SerializeField] private float _totalDuration = 1f;
        [SerializeField] private Ease _totalEase = Ease.OutCubic;

        [SerializeField] private float _totalPunchDuration = 0.2f;
        [SerializeField] private float _totalPunchScale = 1.15f;
        [SerializeField] private Ease _totalPunchEase = Ease.OutBack;

        private Vector2 _addTextStartPosition;
        private Sequence _sequence;

        private void Start()
        {
            _addTextStartPosition = _addCount.rectTransform.anchoredPosition;
        }

        public void SetValue(int count)
        {
            _totalCount.text = count.ToString("D" + _digitCount);
        }

        public void Play(int startCount, int endCount)
        {
            if (startCount == endCount)
                return;

            _sequence?.Kill();

            var addRect = _addCount.rectTransform;
            var totalRect = _totalCount.rectTransform;

            _totalCount.text = startCount.ToString("D" + _digitCount);
            _addCount.text = $"+{endCount - startCount}";

            _addCount.alpha = 0;
            addRect.localScale = Vector3.zero;
            addRect.anchoredPosition = _addTextStartPosition;
            totalRect.localScale = Vector3.one;

            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(_delay);

            // 加算値表示
            _sequence.Append(
                _addCount.DOFade(1, _addFadeDuration)
                    .SetEase(_addFadeInEase)
            );

            _sequence.Join(
                addRect.DOScale(_addScale, _addScaleDuration)
                    .SetEase(_addScaleEase)
            );

            // 加算値が浮きながら消える
            _sequence.Join(
                addRect.DOAnchorPosY(_addTextStartPosition.y + _addMoveDistance, _addMoveDuration)
                    .SetEase(_addMoveEase)
            );

            // 合計値カウントアップ
            _sequence.Join(
                DOVirtual.Int(startCount, endCount, _totalDuration, value =>
                {
                    _totalCount.text = value.ToString("D" + _digitCount);
                })
                .SetEase(_totalEase)
            );

            _sequence.Join(
                _addCount.DOFade(0, _addFadeDuration)
                    .SetDelay(_addMoveDuration - _addFadeDuration)
                    .SetEase(_addFadeOutEase)
            );

            // 最後に合計値を強調
            _sequence.Append(
                totalRect.DOScale(_totalPunchScale, _totalPunchDuration)
                    .SetEase(_totalPunchEase)
            );

            _sequence.Append(
                totalRect.DOScale(1f, _totalPunchDuration)
                    .SetEase(_totalPunchEase)
            );
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}