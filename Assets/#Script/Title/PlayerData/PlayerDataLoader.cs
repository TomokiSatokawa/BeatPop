using Cysharp.Threading.Tasks;
using R3;
using Title.Custom;
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

            _info.OnUpdateData.Subscribe(_ =>   UpdateFile(InfoFileName, _info).Forget());
            _records.OnUpdateData.Subscribe(_ =>   UpdateFile(RecordsFileName, _records).Forget());
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
}