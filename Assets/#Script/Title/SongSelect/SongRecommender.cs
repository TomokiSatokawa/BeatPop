using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Title.Common;
using Title.PlayerData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Title.SongSelect
{
    public class SongRecommender : SingletonMonoBehaviour<SongRecommender>
    {
        [SerializeField] private SongListDataBase _songListData;
        [SerializeField] private ButtonsToggle _difficultyFilter;
        [SerializeField] private RangeUIControl _bgmRangeFilter;
        [SerializeField] private int _recommenderCount;

        public IReadOnlyList<SongSelectData> GetAll()
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

        public IReadOnlyList<SongSelectData> GetRecommender()
        {
            if (PlayerDataLoader.Records.RecentPlayRecords?.Count == 0)
            {
                return GetRandom(_recommenderCount);
            }

            var cost = new Dictionary<SongSelectData, float>();

            foreach (var songData in _songListData.SongDatas)
            {
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    if (songData.Charts.GetChart(difficulty) == null) continue;
                    if (!SongFilter(songData, difficulty)) continue;

                    var songSelectData = new SongSelectData(songData, difficulty);


                    foreach (var record in PlayerDataLoader.Records.RecentPlayRecords)
                    {
                        var recordSongData = _songListData.GetSongData(record.SongIndex);
                        int recordLevel = recordSongData.Charts.GetLevel((Difficulty)record.Difficulty);

                        float bpmCost = Mathf.Abs(recordSongData.BPM - songData.BPM);
                        float levelCost = Mathf.Abs(songData.Charts.GetLevel(difficulty) - recordLevel);
                        float totalCost = levelCost + bpmCost;

                        if (!cost.TryGetValue(songSelectData, out float currentCost) ||
                            totalCost < currentCost)
                        {
                            cost[songSelectData] = totalCost;
                        }
                    }
                }
            }

            return cost.OrderBy(x => x.Value).Take(_recommenderCount).Select(x => x.Key).ToList();
        }

        public IReadOnlyList<SongSelectData> GetRandom(int count)
        {
            var result = new List<SongSelectData>();
            var used = new HashSet<(int SongIndex, Difficulty Difficulty)>();

            int difficultyCount = Enum.GetValues(typeof(Difficulty)).Length;
            int maxCount = _songListData.SongDatas.Count * difficultyCount;

            count = Mathf.Min(count, maxCount);

            int maxAttempts = maxCount * 10;
            int attempts = 0;

            while (result.Count < count && attempts < maxAttempts)
            {
                attempts++;

                int songIndex = Random.Range(0, _songListData.SongDatas.Count);
                var songData = _songListData.SongDatas[songIndex];

                Difficulty difficulty = (Difficulty)Random.Range(0, difficultyCount);

                if (songData.Charts.GetChart(difficulty) == null) continue;
                if (!SongFilter(songData, difficulty)) continue;

                if (!used.Add((songIndex, difficulty)))
                {
                    continue;
                }

                result.Add(new SongSelectData(songData, difficulty));
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

            string normalizedKeyword = NormalizeKeyword(keyword);

            if (string.IsNullOrEmpty(normalizedKeyword))
            {
                return result;
            }

            foreach (var songData in _songListData.SongDatas)
            {
                string songName = NormalizeKeyword(songData.SongName);

                if (!songName.Contains(normalizedKeyword))
                {
                    continue;
                }

                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    if (songData.Charts.GetChart(difficulty) == null)
                    {
                        continue;
                    }

                    if (!SongFilter(songData, difficulty))
                    {
                        continue;
                    }

                    result.Add(new SongSelectData(songData, difficulty));
                }
            }

            return result;
        }

        private string NormalizeKeyword(string value)
        {
            return value
                .Trim()
                .Replace(" ", "")
                .Replace("Å@", "")
                .ToLowerInvariant();
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