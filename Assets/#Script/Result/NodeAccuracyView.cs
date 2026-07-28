using System.Collections.Generic;
using DG.Tweening;
using InGame.Score;
using TMPro;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// ë≈ó¶ï\é¶
    /// </summary>
    public class NodeAccuracyView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<PoolPrefabType, TextMeshProUGUI> _valueTexts;

        private Sequence _sequence;
        public void OnAnimation(IReadOnlyDictionary<PoolPrefabType, HitData> hitCount)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            AddSequence(hitCount, PoolPrefabType.NormalNote);
            AddSequence(hitCount, PoolPrefabType.HoldNoteStart);
            AddSequence(hitCount, PoolPrefabType.HoldNoteEnd);
            AddSequence(hitCount, PoolPrefabType.FlickNote);
        }

        private void AddSequence(IReadOnlyDictionary<PoolPrefabType, HitData> hitCount, PoolPrefabType type)
        {
            var text = _valueTexts[type];

            if (hitCount.TryGetValue(type, out var hitData))
            {
                _sequence.AppendCallback(() =>
                {
                    DOTween.To(
                            () => 0f,
                            value => text.text = $"{value:F1}%",
                            hitData.Accuracy,
                            0.5f)
                        .SetEase(Ease.OutCubic);
                });
            }
            else
            {
                _sequence.AppendCallback(() => text.text = "---");
            }

            // éüÇÃçÄñ⁄Ç÷êiÇﬁÇ‹Ç≈ë“ã@
            _sequence.AppendInterval(0.1f);
        }
    }
}