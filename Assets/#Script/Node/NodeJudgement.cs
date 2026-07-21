using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// îªíËÉfÅ[É^
    /// </summary>
    [CreateAssetMenu(fileName = "NodeJudgement", menuName = "Scriptable Objects/NodeJudgement")]
    public class NodeJudgement : ScriptableObject
    {
        public float ToleranceValue;
        public float DeleteTime;
        public JudgementData[] JudgementDatas;

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
}