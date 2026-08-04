using Cysharp.Threading.Tasks;
using Title.Custom;
using UnityEngine;
namespace Title.PlayerData
{
    /// <summary>
    /// プレイヤーデータを読み込む
    /// </summary>
    public class PlayerDataLoader : MonoBehaviour
    {
        private const string FolderName = "PlayerData";
        private const string InfoFileName = "PlayerInfo.json";
        private const string RecordsFileName = "PlayerRecords.json";

        private PlayerInfo _info;
        public IReadOnlyPlayerInfo Info => _info;
        private PlayerRecords _records;
        public IReadOnlyPlayerRecords Records => _records;

        public async UniTask LoadData()
        {
            _info = await TryGetCreateFile<PlayerInfo>(InfoFileName);
            _records = await TryGetCreateFile<PlayerRecords>(RecordsFileName);
        }

        private async UniTask<T> TryGetCreateFile<T>(string fileName) where T :new()
        {
            string infoFile = "";
            if (!await FileStorage.TryGetText(FolderName, InfoFileName, t => infoFile = t))
            {
              return await CreateFile<T>(fileName);
            }

            return JsonUtility.FromJson<T>(infoFile);
        }
        private async UniTask<T> CreateFile<T>(string fileName) where T : new()
        {
            T fileData = new();
            string json = JsonUtility.ToJson(fileData);
            await FileStorage.CreateFile(FolderName, InfoFileName, json);
            return fileData;
        }
    }
    public class PlayerInfo : IReadOnlyPlayerInfo
    {
        public string Name { get; set; }
    }
    public interface IReadOnlyPlayerInfo
    {
        public string Name { get; }
    }

    public class PlayerRecords : IReadOnlyPlayerRecords
    {
        
    }

    public interface IReadOnlyPlayerRecords
    {

    }
}