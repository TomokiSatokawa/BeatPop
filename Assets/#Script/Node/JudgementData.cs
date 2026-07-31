using TMPro;

namespace InGame.Node
{
    /// <summary>
    /// îªíËÉfÅ[É^
    /// </summary>
    [System.Serializable]
    public class JudgementData : IReadOnlyJudgementData
    {
        public JudgementType Name;
        public float Value;
        public bool ShowEarlyLateText;
        public TMP_ColorGradient TextColor;

        public bool IsComboContinued;
        public bool IsAllPerfectContinued;
        public float ScoreMultiplier;
        public float TapSEVolume;

        JudgementType IReadOnlyJudgementData.Name => Name;
        float IReadOnlyJudgementData.Value => Value;
        bool IReadOnlyJudgementData.ShowEarlyLateText => ShowEarlyLateText;
        TMP_ColorGradient IReadOnlyJudgementData.TextColor => TextColor;
        bool IReadOnlyJudgementData.IsComboContinued => IsComboContinued;
        bool IReadOnlyJudgementData.IsAllPerfectContinued => IsAllPerfectContinued;
        float IReadOnlyJudgementData.ScoreMultiplier => ScoreMultiplier;
        float IReadOnlyJudgementData.TapSEVolume => TapSEVolume;

    }

    public interface IReadOnlyJudgementData
    {
        public JudgementType Name { get; }
        public float Value { get; }

        public bool ShowEarlyLateText { get; }

        public TMP_ColorGradient TextColor { get; }

        public bool IsComboContinued { get; }

        public bool IsAllPerfectContinued { get; }

        public float ScoreMultiplier { get; }

        public float TapSEVolume { get; }
    }
}