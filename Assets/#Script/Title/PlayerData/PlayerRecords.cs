using R3;
using Result.UI;
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
        public IReadOnlyList<PlayRecord> HighScores => _highScores;//TODO:用途が変わったので名前を変える
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
                _highScores.Add(new(songIndex, difficulty, score,ResultType.Clear));
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
        /// プレイ記録を追加する
        /// </summary>
        public void SavePlayResult(SongSelectData selectData, int score, ResultType resultType)
        {
            int songIndex = selectData.SongData.SongID;
            int difficulty = (int)selectData.Difficulty;

            //ハイスコアを探す
            int highScoreIndex = _highScores.FindIndex(x => x.SongIndex == songIndex && x.Difficulty == difficulty);

            if (highScoreIndex == -1)
            {
                SaveAddPlayData(selectData, score, resultType);
                return;
            }

            _highScores[highScoreIndex].AddResult(resultType);
        }

        /// <summary>
        /// プレイ履歴を保存する
        /// </summary>
        public void SaveAddPlayData(SongSelectData selectData, int score,ResultType resultType)
        {
            int songIndex = selectData.SongData.SongID;
            int difficulty = (int)selectData.Difficulty;

            _recentPlayRecords.Add(new(songIndex, difficulty, score, resultType));
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
        public void SavePlayResult(SongSelectData selectData, int score, ResultType resultType);
        public void SaveAddPlayData(SongSelectData selectData, int score, ResultType resultType);
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
        [SerializeField] private bool _fullCombo;
        [SerializeField] private bool _allPerfect;

        public int SongIndex => _songIndex;
        public int Difficulty => _difficulty;
        public int Score => _score;
        public bool FullCombo => _fullCombo;
        public bool AllPerfect => _allPerfect;

        public PlayRecord(int songIndex, int difficulty, int score,ResultType resultType)
        {
            _songIndex = songIndex;
            _difficulty = difficulty;
            _score = score;
            _fullCombo = resultType == ResultType.FullCombo;
            _allPerfect = resultType == ResultType.AllPerfect;
        }

        /// <summary>
        /// スコアを更新する
        /// </summary>
        public void UpdateScore(int score)
        {
            _score = score;
        }

        /// <summary>
        /// 結果を追加する
        /// </summary>
        public void AddResult(ResultType resultType)
        {
            _fullCombo |= resultType == ResultType.FullCombo;
            _allPerfect |= resultType == ResultType.AllPerfect;
        }
    }
}