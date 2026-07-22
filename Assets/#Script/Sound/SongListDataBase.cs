using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// ã»ÉfÅ[É^àÍóó
    /// </summary>
    [CreateAssetMenu(fileName = "SongListDataBase", menuName = "Scriptable Objects/SongListDataBase")]
    public class SongListDataBase : ScriptableObject
    {
        [SerializeField] private List<SongData> _songDatas;
        public IReadOnlyList<IReadOnlySongData> SongDatas => _songDatas;

        private Dictionary<int, SongData> _songIdDataMap;

        private void Initialize()
        {
            _songIdDataMap = _songDatas.ToDictionary(x => x.SongID);
        }

        public IReadOnlySongData GetSongData(int id)
        {
            if (_songIdDataMap == null || _songIdDataMap.Count == 0)
                Initialize();

            return _songIdDataMap.TryGetValue(id, out var song) ? song : null;
        }
    }
}