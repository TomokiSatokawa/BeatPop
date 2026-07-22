using UnityEngine;

namespace Title.Custom
{
    public class CustomPatternLoader : MonoBehaviour
    {
        [SerializeField] private CustomSoundData _customSoundData;
        public PatternJsonData GetDefaultPattern()
        {
            PatternJsonData pattern = new();
            pattern.SoundPattern = _customSoundData.GetDefaultCustom();
            return pattern;
        }
    }

    public class PatternJsonData
    {
        public string PatternName = "デフォルト";
        public string FillName;
        public bool IsSelect;
        public CustomSoundPattern SoundPattern;
    }
}