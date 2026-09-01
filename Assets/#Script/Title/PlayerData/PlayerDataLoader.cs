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
        private const string SettingsFileName = "Settings.json";

        [SerializeField] private bool _isAutoLoad;

        private static PlayerInfo _info;
        private static PlayerRecords _records;
        private static SettingsData _settingsData;
        public static IReadOnlyPlayerInfo Info => _info;
        public static IReadOnlyPlayerRecords Records => _records;
        public static IReadOnlySettingsData Settings => _settingsData;

        protected override void OnAwake()
        {
            if (_isAutoLoad)
            {
                LoadData().Forget();
            }
        }

        public async UniTask LoadData()
        {
            _info = await TryGetCreateFile<PlayerInfo>(InfoFileName);
            _records = await TryGetCreateFile<PlayerRecords>(RecordsFileName);
            _settingsData = await TryGetCreateFile<SettingsData>(SettingsFileName);

            _info.OnUpdateData.Subscribe(_ =>   UpdateFile(InfoFileName, _info).Forget());
            _records.OnUpdateData.Subscribe(_ =>   UpdateFile(RecordsFileName, _records).Forget());
            _settingsData.OnUpdateData.Subscribe(_ =>   UpdateFile(SettingsFileName, _settingsData).Forget());
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