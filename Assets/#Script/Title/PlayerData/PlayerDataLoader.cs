using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using Title.Custom;
using Title.SongSelect;
using UnityEngine;

namespace Title.PlayerData
{
    /// <summary>
    /// プレイヤーデータを読み込む
    /// </summary>
    public class PlayerDataLoader : SingletonPersistent<PlayerDataLoader>
    {
        private const string FolderName = "PlayerData";
        private const string InfoFileName = "PlayerInfo.json";
        private const string RecordsFileName = "PlayerRecords.json";

        private static PlayerInfo _info;
        public static IReadOnlyPlayerInfo Info => _info;
        private static PlayerRecords _records;
        public static IReadOnlyPlayerRecords Records => _records;

        public async UniTask LoadData()
        {
            _info = await TryGetCreateFile<PlayerInfo>(InfoFileName);
            _records = await TryGetCreateFile<PlayerRecords>(RecordsFileName);

            _records.OnUpdate.Subscribe(_ =>
            {
                UpdateFile(RecordsFileName, _records).Forget();
                Debug.Log("A");
            });
        }

        private async UniTask<T> TryGetCreateFile<T>(string fileName) where T : new()
        {
            string infoFile = "";
            if (!await FileStorage.TryGetText(FolderName, fileName, t => infoFile = t))
            {
                return await CreateFile<T>(fileName);
            }

            return JsonUtility.FromJson<T>(infoFile);
        }
        private async UniTask<T> CreateFile<T>(string fileName) where T : new()
        {
            T fileData = new();
            string json = JsonUtility.ToJson(fileData, true);
            await FileStorage.CreateFile(FolderName, fileName, json);
            return fileData;
        }

        private async UniTask UpdateFile<T>(string fileName, T data)
        {
            string json = JsonUtility.ToJson(data, true);
            await FileStorage.UpdateFile(FolderName, fileName, json);
        }
    }

    [System.Serializable]
    public class PlayerInfo : IReadOnlyPlayerInfo
    {
        public string Name { get; set; }
    }

    public interface IReadOnlyPlayerInfo
    {
        public string Name { get; }
    }

    [System.Serializable]
    public class PlayerRecords : IReadOnlyPlayerRecords
    {
        private const int RecordsCount = 10;
        [SerializeField] private List<PlayRecord> _highScores = new();
        [SerializeField] private List<PlayRecord> _recentPlayRecords = new();
        public IReadOnlyList<PlayRecord> HighScores => _highScores;
        public IReadOnlyList<PlayRecord> RecentPlayRecords => _recentPlayRecords;

        private Subject<Unit> _onUpdate = new();
        public Observable<Unit> OnUpdate => _onUpdate;

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
                _onUpdate.OnNext(Unit.Default);
                return true;
            }

            //ハイスコアを更新する
            if (_highScores[highScoreIndex].Score <= score)
            {
                _highScores[highScoreIndex].UpdateScore(score);
                highScore = score;
                _onUpdate.OnNext(Unit.Default);
                return true;
            }

            highScore = _highScores[highScoreIndex].Score;
            return false;
        }

        public void SavePlayData(SongSelectData selectData, int score)
        {
            int songIndex = selectData.SongData.SongID;
            int difficulty = (int)selectData.Difficulty;

            _recentPlayRecords.Add(new(songIndex, difficulty, score));
            TrimHead(RecordsCount);
            _onUpdate.OnNext(Unit.Default);
        }

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

        public void UpdateScore(int score)
        {
            _score = score;
        }
    }
}