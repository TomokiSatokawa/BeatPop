using System.Collections.Generic;
using Sound;
using UnityEngine;
using static SerializableDictionary<string,Sound.SESoundType>;

[CreateAssetMenu(fileName = "CustomSoundData", menuName = "Scriptable Objects/CustomSoundData")]
public class CustomSoundData : ScriptableObject
{
    [SerializeField] private SerializableDictionary<string,SESoundType> _holdSE;
    [SerializeField] private SerializableDictionary<string,SESoundType> _tapSE;
    [SerializeField] public int _normalDefault;
    [SerializeField] public int _flickDefault;
    [SerializeField] public int _longStartDefault;
    [SerializeField] public int _longFillDefault;
    [SerializeField] public int _longEndDefault;
    public IReadOnlyList<KeyPair> TapSE => _tapSE.Items;
    public IReadOnlyList<KeyPair> HoldSE => _holdSE.Items;

    public CustomSoundPattern GetDefaultCustom()
    {
        var result = new CustomSoundPattern();

        result.NormalSE = _normalDefault;
        result.FlickSE = _flickDefault;
        result.HoldStart = _longStartDefault;
        result.HoldFill = _longFillDefault;
        result.HoldEnd = _longEndDefault;

        return result;
    }
}
[System.Serializable]
public struct CustomSoundPattern
{
    public int NormalSE;
    public int FlickSE;
    public int HoldStart;
    public int HoldFill;
    public int HoldEnd;
}