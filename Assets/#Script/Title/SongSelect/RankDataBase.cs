using System.Collections.Generic;
using UnityEngine;

namespace Title.SongSelect
{
    [CreateAssetMenu(fileName = "RankDataBase", menuName = "Scriptable Objects/RankDataBase")]
    public class RankDataBase : ScriptableObject
    {
        [SerializeField] private List<RankData> _rankValue;
        public IReadOnlyList<RankData> RankValue => _rankValue;
        public RankData GetRank(float rate)
        {
            foreach (var kv in _rankValue)
            {
                if (kv.Rate <= rate)
                {
                    return kv;
                }
            }
            Debug.LogError("Rank is not found");
            return null;
        }
        [System.Serializable]
        public class RankData
        {
           public Sprite Image;
            public AudioClip Clip;
            public float Rate;
        }
    }
}