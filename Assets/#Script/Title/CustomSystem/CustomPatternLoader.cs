using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// デフォルトカスタムデータを取得
    /// </summary>
    public class CustomPatternLoader : MonoBehaviour
    {
        [SerializeField] private CustomSoundData _customSoundData;
        [SerializeField] private CustomColorData _customColorData;
        [SerializeField] private CustomStageData _customStageData;
        public PatternJsonData GetDefaultPattern()
        {
            PatternJsonData pattern = new();
            pattern.SoundPattern = _customSoundData.GetDefault();
            pattern.ColorPattern = _customColorData.GetDefault();
            pattern.ChartPattern = default;
            pattern.JudgePattern = default;
            pattern.SpeedPattern = _customStageData.GetDefault();
            return pattern;
        }
    }

    public class PatternJsonData
    {
        public string PatternName = "デフォルト";
        public string FileName;
        public bool IsSelect;
        public bool IsDefault = false;
        public CustomSoundPattern SoundPattern;
        public CustomChartPattern ChartPattern;
        public CustomColorPattern ColorPattern;
        public CustomJudgePattern JudgePattern;
        public CustomStagePattern SpeedPattern;
        public CustomOtherPattern OtherPattern;
    }
}