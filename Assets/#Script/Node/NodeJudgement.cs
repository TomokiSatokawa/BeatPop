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
                return judgement;
            }
        }
        return null;
    }
}
public interface IReadOnlyJudgementData
{
    JudgementType Name { get; }
    float Value { get; }

    bool ShowEarlyLateText { get; }

    GameObject Prefab { get; }

    bool IsComboContinued { get; }

    bool IsAllPerfectContinued { get; }

    int Score { get; }
}

[System.Serializable]
public class JudgementData : IReadOnlyJudgementData
{
    public JudgementType Name;
    public float Value;
    public bool ShowEarlyLateText;
    public GameObject Prefab;

    public bool IsComboContinued;
    public bool IsAllPerfectContinued;
    public int Score;
    JudgementType IReadOnlyJudgementData.Name => Name;
    float IReadOnlyJudgementData.Value => Value;

    bool IReadOnlyJudgementData.ShowEarlyLateText => ShowEarlyLateText;

    GameObject IReadOnlyJudgementData.Prefab => Prefab;

    bool IReadOnlyJudgementData.IsComboContinued => IsComboContinued;

    bool IReadOnlyJudgementData.IsAllPerfectContinued => IsAllPerfectContinued;
    int IReadOnlyJudgementData.Score => Score;
}
public enum JudgementType
{
    Perfect, Great, Good, Bad,Miss
}