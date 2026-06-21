using UnityEngine;

public class CustomPatternLoader : MonoBehaviour
{
    [SerializeField] private CustomSoundData _customSoundData;
    public string  GetDefaultPattern()
    {
        PatternJsonData pattern = new();
        pattern.SoundPattern = _customSoundData.GetDefaultCustom();
        return JsonUtility.ToJson(pattern,true);
    }
}

public class PatternJsonData
{
    public CustomSoundPattern SoundPattern;
}