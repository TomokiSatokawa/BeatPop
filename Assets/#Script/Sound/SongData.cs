using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "Scriptable Objects/SongData")]
public class SongData : ScriptableObject, IReadOnlySongData
{
    [SerializeField] private string _songName;
    [SerializeField] private int _songID;
    [SerializeField] private TextAsset _nodeDataJson;
    [SerializeField] private AudioClip _audioClip;

    public string SongName => _songName;
    public int SongID => _songID;
    public TextAsset NodeDataJson => _nodeDataJson;
    public AudioClip Audio => _audioClip;
}
public interface IReadOnlySongData
{
    public string SongName { get; }
    public int SongID { get; }
    public TextAsset NodeDataJson { get; }
    public AudioClip Audio { get; }
}