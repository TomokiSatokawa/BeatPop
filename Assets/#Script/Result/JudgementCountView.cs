using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InGame.Node;
using TMPro;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// îªíËå¬êîUIÇÃView
    /// </summary>
    public class JudgementCountView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<JudgementType, TextMeshProUGUI> _judgeValue;
        [SerializeField] private float _showInterval;
        private Sequence _sequence; 

        public void OnAnimation(IReadOnlyDictionary<IReadOnlyJudgementData, int> judgeCount)
        {
            _sequence?.Kill(true);
            _sequence = DOTween.Sequence();

            AddSequence(judgeCount, JudgementType.Perfect);
            AddSequence(judgeCount, JudgementType.Great);
            AddSequence(judgeCount, JudgementType.Good);
            AddSequence(judgeCount, JudgementType.Bad);
            AddSequence(judgeCount, JudgementType.Miss);

            _sequence.Play();
        }

        private void AddSequence(IReadOnlyDictionary<IReadOnlyJudgementData, int> judgeCount, JudgementType type)
        {
            if (!_judgeValue.TryGetValue(type, out var text))
            {
               Debug.LogError($"[JudgementCountAnimation] Text not found : {type}");
                return;
            }
            var result = judgeCount.FirstOrDefault(x => x.Key.Name == type);
            int count = result.Value;
            if (EqualityComparer<IReadOnlyJudgementData>.Default.Equals(result.Key, default))
            {
                count = 0;
            }
            text.text = "";
            _sequence.Join(ScrambleText(text, ((int)type + 1) * _showInterval).OnComplete(() => text.text = count.ToString()));
        }

        private Tween ScrambleText(TextMeshProUGUI text, float duration)
        {
            return DOTween.To(() => 0, _ =>
            {
                int random = Random.Range(0, 999999);
                text.text = random.ToString("N0");
            }, 1, duration);
        }
    }

}