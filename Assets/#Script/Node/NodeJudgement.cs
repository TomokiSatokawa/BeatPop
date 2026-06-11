using UnityEngine;

    [CreateAssetMenu(fileName = "NodeJudgement", menuName = "Scriptable Objects/NodeJudgement")]
public class NodeJudgement : ScriptableObject
{
    public float ToleranceValue;
    public float DeleteTime;
    public JudgementData[] JudgementDatas;

  

    public JudgementData JudgementDifference(float difference)
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
[System.Serializable]
public class JudgementData
{
    public string Name;
    public float Value;
    public bool ShowEarlyLateText;
    public GameObject Prefab;
}