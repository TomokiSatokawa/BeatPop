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

        //‚È‚¯‚ê‚Îˆê”ÔÅ’á•]‰¿‚ð•Ô‚·
        return JudgementDatas[JudgementDatas.Length - 1];
    }
}
public interface IReadOnlyJudgementData
{
    public JudgementType Name { get; }
    public float Value { get; }

    public bool ShowEarlyLateText { get; }

    public Color TextColor { get; }

    public bool IsComboContinued { get; }

    public bool IsAllPerfectContinued { get; }

    public int Score { get; }

    public float TapSEVolume { get; }
}

[System.Serializable]
public class JudgementData : IReadOnlyJudgementData
{
    public JudgementType Name;
    public float Value;
    public bool ShowEarlyLateText;
    public Color TextColor;

    public bool IsComboContinued;
    public bool IsAllPerfectContinued;
    public int Score;
    public float TapSEVolume;

    JudgementType IReadOnlyJudgementData.Name => Name;
    float IReadOnlyJudgementData.Value => Value;
    bool IReadOnlyJudgementData.ShowEarlyLateText => ShowEarlyLateText;
    Color IReadOnlyJudgementData.TextColor => TextColor;
    bool IReadOnlyJudgementData.IsComboContinued => IsComboContinued;
    bool IReadOnlyJudgementData.IsAllPerfectContinued => IsAllPerfectContinued;
    int IReadOnlyJudgementData.Score => Score;
    float IReadOnlyJudgementData.TapSEVolume => TapSEVolume;

}
public enum JudgementType
{
    Perfect, Great, Good, Bad, Miss
}