using System;
using System.Collections.Generic;
using UnityEngine;

public class SongRecommender : SingletonMonoBehaviour<SongRecommender>
{
    [SerializeField] private SongListDataBase _songListData;
    public IReadOnlyList<SongSelectData> GetRecommendation()
    {
        var result = new List<SongSelectData>();
        foreach (var songData in _songListData.SongDates)
        {
            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                if (songData.Charts.GetChart(difficulty) == null) continue;

                result.Add(new SongSelectData(songData, difficulty));
            }
        }
        return result;
    }

    public IReadOnlyList<SongSelectData> GetKeywordSong(string keyword)
    {
        var result = new List<SongSelectData>();

        foreach (var songData in _songListData.SongDates)
        {
            if(!songData.SongName.Contains(keyword)) continue;
            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                if (songData.Charts.GetChart(difficulty) == null) continue;

                result.Add(new SongSelectData(songData, difficulty));
            }
        }

        return result;
    }
}
public struct SongSelectData
{
    public readonly IReadOnlySongData SongData;
    public Difficulty Difficulty { get; private set; }
    public SongSelectData(IReadOnlySongData songData, Difficulty difficulty)
    {
        SongData = songData;
        Difficulty = difficulty;
    }
    public void ChangeDifficulty(Difficulty difficulty)
    {
        Difficulty = difficulty;
    }
    public TextAsset GetNodeJson()
    {
        return SongData.Charts.GetChart(Difficulty);
    }
}