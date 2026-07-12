using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Scriptable Objects/SongData")]
public class SongData : ScriptableObject, IReadOnlySongData
{
    [SerializeField] private string _songName;
    [SerializeField] private int _songID;
    [SerializeField] private float _bpm;
    [SerializeField] private int _maxScore;
    [Space(10)]
    [SerializeField] private SongChartSet _charts;
    [Space(10)]
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _stageTimeOffSet;
    [SerializeField] private float _editorTimeOffSet;
    [Space(10)]
    [SerializeField] private TextAsset _stageEffectData;

    public string SongName => _songName;
    public int SongID => _songID;
    public float BPM => _bpm;
    public int MaxScore => _maxScore;
    public SongChartSet Charts => _charts;
    public AudioClip Audio => _audioClip;
    public float StageTimeOffSet => _stageTimeOffSet;
    public float EditorTimeOffSet => _editorTimeOffSet;
    public TextAsset StageEffectData => _stageEffectData;
}
public interface IReadOnlySongData
{
    public string SongName { get; }
    public int SongID { get; }
    public float BPM { get; }
    public int MaxScore { get; }    
    public SongChartSet Charts { get; }
    public AudioClip Audio { get; }
    public float StageTimeOffSet { get; }
    public float EditorTimeOffSet { get; }
    public TextAsset StageEffectData { get; }
}

[System.Serializable]
public struct SongChartSet
{
    [SerializeField] private int EasyLevel;
    [SerializeField] private TextAsset EasyJson;
    [Space(10)]
    [SerializeField] private int NormalLevel;
    [SerializeField] private TextAsset NormalJson;
    [Space(10)]
    [SerializeField] private int HardLevel;
    [SerializeField] private TextAsset HardJson;
    [Space(10)]
    [SerializeField] private int ExpertLevel;
    [SerializeField] private TextAsset ExpertJson;

    public TextAsset GetChart(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return EasyJson;
            case Difficulty.Normal:
                return NormalJson;
            case Difficulty.Hard:
                return HardJson;
            case Difficulty.Expert:
                return ExpertJson;
        }
        Debug.LogError("”ÍˆÍŠO“ïˆÕ“x");
        return null;
    }
    public int GetLevel(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return EasyLevel;
            case Difficulty.Normal:
                return NormalLevel;
            case Difficulty.Hard:
                return HardLevel;
            case Difficulty.Expert:
                return ExpertLevel;
            default:
                return 0;
        }
    }
}