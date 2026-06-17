using UnityEngine;

    [CreateAssetMenu(fileName = "NodeJudgement", menuName = "Scriptable Objects/NodeJudgement")]
public class NodeJudgement : ScriptableObject
{
    public float ToleranceValue;
    public float DeleteTime;
    public JudgementData[] JudgementDatas;

  

    public IReadOnlyJudgementData JudgementDifference(float difference)
    {
        float absDiff = Mathf.Abs(difference);

        foreach (var judgement in JudgementDatas)
        {
            if (absDiff <= judgement.Value)
            {
                string result = judgement.Name;
                if (judgement.ShowEarlyLateText)
                {
                    result += "\n" +( difference > 0 ? "fast" : "late");
                }

                return judgement;
            }
        }
        return null;
    }
}
public interface IReadOnlyJudgementData
{
    string Name { get; }
    float Value { get; }

    bool ShowEarlyLateText { get; }

    GameObject Prefab { get; }

    bool IsComboContinued { get; }

    bool IsAllPerfectContinued { get; }
}

[System.Serializable]
public class JudgementData : IReadOnlyJudgementData
{
    public string Name;
    public float Value;
    public bool ShowEarlyLateText;
    public GameObject Prefab;

    public bool IsComboContinued;
    public bool IsAllPerfectContinued;

    string IReadOnlyJudgementData.Name => Name;
    float IReadOnlyJudgementData.Value => Value;

    bool IReadOnlyJudgementData.ShowEarlyLateText => ShowEarlyLateText;

    GameObject IReadOnlyJudgementData.Prefab => Prefab;

    bool IReadOnlyJudgementData.IsComboContinued => IsComboContinued;

    bool IReadOnlyJudgementData.IsAllPerfectContinued => IsAllPerfectContinued;
}