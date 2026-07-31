using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// îªíËÉfÅ[É^
    /// </summary>
    [CreateAssetMenu(fileName = "NodeJudgement", menuName = "Scriptable Objects/NodeJudgement")]
    public class NodeJudgement : ScriptableObject, IReadOnlyNodeJudgement
    {
        [SerializeField] private float ToleranceValue;
        [SerializeField] private float DeleteTime;
        [SerializeField] private float MaxScore;
        [SerializeField] private float BaseScore;
        [SerializeField] private JudgementData[] JudgementDatas;

        float IReadOnlyNodeJudgement.MaxScore => MaxScore;
        float IReadOnlyNodeJudgement.BaseScore => BaseScore;

        public IReadOnlyJudgementData GetJudgement(float difference)
        {
            if (JudgementDatas == null || JudgementDatas.Length == 0)
            {
                Debug.LogError("[NodeJudgement] JudgementDatas is empty.");
                return null;
            }

            float absDiff = Mathf.Abs(difference);

            foreach (var judgement in JudgementDatas)
            {
                if (absDiff <= judgement.Value)
                {
                    return judgement;
                }
            }

            //Ç»ÇØÇÍÇŒàÍî‘ç≈í·ï]âøÇï‘Ç∑
            return JudgementDatas[JudgementDatas.Length - 1];
        }
    }

    public interface IReadOnlyNodeJudgement
    {
        public float MaxScore { get; }
        public float BaseScore { get; }
    }

    public enum JudgementType
    {
        PERFECT, GREAT, GOOD, BAD, MISS
    }
}