using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// デフォルトカスタムデータを取得
    /// </summary>
    public class CustomPatternLoader : MonoBehaviour
    {
        [SerializeField] private CustomSoundData _customSoundData;
        public PatternJsonData GetDefaultPattern()
        {
            PatternJsonData pattern = new();
            pattern.SoundPattern = _customSoundData.GetDefaultCustom();
            pattern.ChartPattern = default;
            return pattern;
        }
    }

    public class PatternJsonData
    {
        public string PatternName = "デフォルト";
        public string FileName;
        public bool IsSelect;
        public CustomSoundPattern SoundPattern;
        public CustomChartPattern ChartPattern;
    }
}