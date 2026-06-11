using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongDataBase", menuName = "Scriptable Objects/SongDataBase")]
public class SongDataBase : ScriptableObject
{
    public List<AudioClip> _audioClipList;
}
