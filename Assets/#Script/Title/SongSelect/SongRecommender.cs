using System;
using System.Collections.Generic;
using Common;
using Title.Common;
using Title.PlayerData;
using UnityEngine;

namespace Title.SongSelect
{
    public class SongRecommender : SingletonMonoBehaviour<SongRecommender>
    {
        [SerializeField] private SongListDataBase _songListData;
        [SerializeField] private ButtonsToggle _difficultyFilter;
        [SerializeField] private RangeUIControl _bgmRangeFilter;

        public IReadOnlyList<SongSelectData> GetRecommendation()
        {
            var result = new List<SongSelectData>();
            foreach (var songData in _songListData.SongDatas)
            {
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    if (songData.Charts.GetChart(difficulty) == null) continue;
                    if (!SongFilter(songData, difficulty)) continue;

                    result.Add(new SongSelectData(songData, difficulty));
                }
            }
            return result;
        }

        public IReadOnlyList<SongSelectData> GetPlayHistory()
        {
            var result = new List<SongSelectData>();
            var added = new HashSet<(int songIndex, Difficulty difficulty)>();

            foreach (var history in PlayerDataLoader.Records.RecentPlayRecords)
            {
                var difficulty = (Difficulty)history.Difficulty;

                if (!added.Add((history.SongIndex, difficulty)))
                    continue;

                var songData = _songListData.GetSongData(history.SongIndex);
                result.Add(new SongSelectData(songData, difficulty));
            }

            return result;
        }

        private bool SongFilter(IReadOnlySongData songData, Difficulty difficulty)
        {
            if (!_difficultyFilter.IsOn[(int)difficulty]) return false;

            float bpm = songData.BPM;
            if (bpm < _bgmRangeFilter.RangeValueMin || bpm > _bgmRangeFilter.RangeValueMax) return false;

            return true;
        }

        public IReadOnlyList<SongSelectData> GetKeywordSong(string keyword)
        {
            var result = new List<SongSelectData>();

            foreach (var songData in _songListData.SongDatas)
            {
                if (!songData.SongName.Contains(keyword)) continue;
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    if (songData.Charts.GetChart(difficulty) == null) continue;
                    if (!SongFilter(songData, difficulty)) continue;

                    result.Add(new SongSelectData(songData, difficulty));
                }
            }

            return result;
        }
    }
    public struct SongSelectData
    {
        public readonly IReadOnlySongData SongData;
        public readonly Difficulty Difficulty;
        public SongSelectData(IReadOnlySongData songData, Difficulty difficulty)
        {
            SongData = songData;
            Difficulty = difficulty;
        }

        public TextAsset GetNodeJson()
        {
            return SongData.Charts.GetChart(Difficulty);
        }
    }
}