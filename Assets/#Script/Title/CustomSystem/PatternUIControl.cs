using UnityEngine;

public class PatternUIControl : MonoBehaviour
{
    private PatternJsonData _patternData;
    public void SetData(PatternJsonData pattern)
    {
        _patternData = pattern;
    }
}
