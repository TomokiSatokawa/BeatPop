using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// 判定データ
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

            //なければ一番最低評価を返す
            return JudgementDatas[JudgementDatas.Length - 1];
        }
    }

    public interface IReadOnlyNodeJudgement
    {
        public float MaxScore { get; }
        public float BaseScore { get; }
    }



    //TODO:以下別ファイルに分離
    public interface IReadOnlyJudgementData
    {
        public JudgementType Name { get; }
        public float Value { get; }

        public bool ShowEarlyLateText { get; }

        public Color TextColor { get; }

        public bool IsComboContinued { get; }

        public bool IsAllPerfectContinued { get; }

        public float ScoreMultiplier { get; }

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
        public float ScoreMultiplier;
        public float TapSEVolume;

        JudgementType IReadOnlyJudgementData.Name => Name;
        float IReadOnlyJudgementData.Value => Value;
        bool IReadOnlyJudgementData.ShowEarlyLateText => ShowEarlyLateText;
        Color IReadOnlyJudgementData.TextColor => TextColor;
        bool IReadOnlyJudgementData.IsComboContinued => IsComboContinued;
        bool IReadOnlyJudgementData.IsAllPerfectContinued => IsAllPerfectContinued;
        float IReadOnlyJudgementData.ScoreMultiplier => ScoreMultiplier;
        float IReadOnlyJudgementData.TapSEVolume => TapSEVolume;

    }
    public enum JudgementType
    {
        Perfect, Great, Good, Bad, Miss
    }
}