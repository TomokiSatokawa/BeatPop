using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SongListDataBase", menuName = "Scriptable Objects/SongListDataBase")]
public class SongListDataBase : ScriptableObject
{
    [SerializeField] private List<SongData> _songDates;
    public IReadOnlyList<IReadOnlySongData> SongDates => _songDates;
    public IReadOnlySongData GetSongData(int id)
    {
        return _songDates.Where(x => x.SongID == id).FirstOrDefault();
    }
}
