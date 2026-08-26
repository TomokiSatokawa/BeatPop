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
        public PatternJsonData GetDefaultPattern()
        {
            PatternJsonData pattern = new();
            pattern.SoundPattern = _customSoundData.GetDefaultCustom();
            pattern.ColorPattern = _customColorData.GetDefault();
            pattern.ChartPattern = default;
            pattern.JudgePattern = default;
            pattern.SpeedPattern = default;
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
    }
}