using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InGame.Node;
using TMPro;
using UnityEngine;

public class JudgementCountControl : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<JudgementType, TextMeshProUGUI> _judgeValue;
    [SerializeField] private float _showInterval;
    private Sequence _sequence;
    public void OnAnimation(IReadOnlyDictionary<IReadOnlyJudgementData, int> judgeCount)
    {
        _sequence?.Kill(true);
        _sequence = DOTween.Sequence();


        void AddSequence(JudgementType type)
        {
            if (!_judgeValue.TryGetValue(type, out var text))
            {
                Debug.LogError("Text not found");
            }
            var result = judgeCount.FirstOrDefault(x => x.Key.Name == type);
            int count = result.Value;
            if (EqualityComparer<IReadOnlyJudgementData>.Default.Equals(result.Key, default))
            {
                count = 0;
            }
            text.text = "";
            _sequence.Join(ScrambleText(text, ((int)type + 1) * _showInterval).OnComplete(() => text.text = count.ToString()));
            ;
        }

        AddSequence(JudgementType.Perfect);
        AddSequence(JudgementType.Great);
        AddSequence(JudgementType.Good);
        AddSequence(JudgementType.Bad);
        AddSequence(JudgementType.Miss);

        _sequence.Play();

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
