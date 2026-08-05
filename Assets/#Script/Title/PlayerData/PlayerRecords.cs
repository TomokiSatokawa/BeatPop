using R3;
using System.Collections.Generic;
using Title.SongSelect;
using UnityEngine;

namespace Title.PlayerData
{
    /// <summary>
    /// プレイ記録
    /// </summary>
    [System.Serializable]
    public class PlayerRecords : IReadOnlyPlayerRecords
    {
        private const int RecordsCount = 10;
        [SerializeField] private List<PlayRecord> _highScores = new();
        [SerializeField] private List<PlayRecord> _recentPlayRecords = new();
        public IReadOnlyList<PlayRecord> HighScores => _highScores;
        public IReadOnlyList<PlayRecord> RecentPlayRecords => _recentPlayRecords;

        private Subject<Unit> _onUpdateData = new();
        /// <summary>データ更新時</summary>
        public Observable<Unit> OnUpdateData => _onUpdateData;

        /// <summary>
        /// ハイスコアを更新する
        /// </summary>
        /// <returns>ハイスコアだったか</returns>
        public bool SaveHighScore(SongSelectData selectData, int score, out int highScore)
        {
            int songIndex = selectData.SongData.SongID;
            int difficulty = (int)selectData.Difficulty;

            //ハイスコアを探す
            int highScoreIndex = _highScores.FindIndex(x => x.SongIndex == songIndex && x.Difficulty == difficulty);

            //ハイスコアがない
            if (highScoreIndex == -1)
            {
                _highScores.Add(new(songIndex, difficulty, score));
                highScore = score;
                _onUpdateData.OnNext(Unit.Default);
                return true;
            }

            //ハイスコアを更新する
            if (_highScores[highScoreIndex].Score <= score)
            {
                _highScores[highScoreIndex].UpdateScore(score);
                highScore = score;
                _onUpdateData.OnNext(Unit.Default);
                return true;
            }

            highScore = _highScores[highScoreIndex].Score;
            return false;
        }

        /// <summary>
        /// プレイ履歴を保存する
        /// </summary>
        public void SavePlayData(SongSelectData selectData, int score)
        {
            int songIndex = selectData.SongData.SongID;
            int difficulty = (int)selectData.Difficulty;

            _recentPlayRecords.Add(new(songIndex, difficulty, score));
            TrimHead(RecordsCount);
            _onUpdateData.OnNext(Unit.Default);
        }

        /// <summary>
        /// 保存数を指定個数にする
        /// </summary>
        private void TrimHead(int maxCount)
        {
            while (_recentPlayRecords.Count > maxCount)
            {
                _recentPlayRecords.RemoveAt(0);
            }
        }
    }

    public interface IReadOnlyPlayerRecords
    {
        public IReadOnlyList<PlayRecord> HighScores { get; }
        public IReadOnlyList<PlayRecord> RecentPlayRecords { get; }
        public bool SaveHighScore(SongSelectData selectData, int score, out int highScore);
        public void SavePlayData(SongSelectData selectData, int score);
    }

    /// <summary>
    /// プレイ記録データ
    /// </summary>
    [System.Serializable]
    public struct PlayRecord
    {
        [SerializeField] private int _songIndex;
        [SerializeField] private int _difficulty;
        [SerializeField] private int _score;

        public int SongIndex => _songIndex;
        public int Difficulty => _difficulty;
        public int Score => _score;

        public PlayRecord(int songIndex, int difficulty, int score)
        {
            _songIndex = songIndex;
            _difficulty = difficulty;
            _score = score;
        }

        /// <summary>
        /// スコアを更新する
        /// </summary>
        public void UpdateScore(int score)
        {
            _score = score;
        }
    }
}