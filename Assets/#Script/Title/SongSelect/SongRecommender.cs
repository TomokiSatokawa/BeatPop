using System.Collections.Generic;
using UnityEngine;

public class SongRecommender : SingletonMonoBehaviour<SongRecommender>
{
    [SerializeField] private SongListDataBase _songListData;
    public IReadOnlyList<IReadOnlySongData> GetRecommendation()
    {
        return _songListData.SongDates;
    }
}
