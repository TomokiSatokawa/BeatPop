using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Title.Custom
{
    /// <summary>
    /// カスタムデータJsonの管理
    /// </summary>
    public class CustomDataLoader : SingletonPersistent<CustomDataLoader>
    {
        [SerializeField] private CustomPatternLoader _patternLoader;
        [SerializeField] private bool _isAutoLoad;

        private ManifestData _manifestData;
        private const string ManifestFileName = "manifest.json";
        private const string FolderName = "CustomData";

        protected override void OnAwake()
        {
            if (_isAutoLoad)
            {
                LoadManifest().Forget();
            }
        }

        /// <summary>
        /// Manifestを読み込む
        /// </summary>
        public async UniTask LoadManifest()
        {
            string manifestJson = "";

            //manifestを作る
            if (!await FileStorage.TryGetText(FolderName,ManifestFileName, t => manifestJson = t))
            {
                manifestJson = await CreateDefaultManifest();
                await FileStorage.CreateFile(FolderName, ManifestFileName, manifestJson);
            }

            _manifestData = JsonUtility.FromJson<ManifestData>(manifestJson);

            foreach (var filePath in _manifestData.FileName)
            {
                if (!await FileStorage.TryGetText(FolderName, filePath, null))
                {
                    Debug.LogError($"ファイル破損 {filePath}");
                }   

                await UniTask.Yield();
            }
        }

        /// <summary>
        /// すべてのカスタムデータを取得
        /// </summary>
        public async UniTask<PatternJsonData[]> GetAllCustomPattern()
        {
            var result = new PatternJsonData[_manifestData.FileName.Length];
            for (int i = 0; i < _manifestData.FileName.Length; i++)
            {
                string patternJson = "";
                string fileName = _manifestData.FileName[i];
                if (!await FileStorage.TryGetText(FolderName, fileName, t => patternJson = t))
                {
                    Debug.LogError($"{fileName}");
                    continue;
                }
                result[i] = JsonUtility.FromJson<PatternJsonData>(patternJson);
            }
            return result;
        }

        /// <summary>
        /// パターンを追加
        /// </summary>
        public async UniTask AddPattern(PatternJsonData patternData)
        {
            Array.Resize(ref _manifestData.FileName, _manifestData.FileName.Length + 1);

            string filName = $"song_{(_manifestData.FileName.Length - 1):D4}.json";
            _manifestData.FileName[_manifestData.FileName.Length - 1] = filName;
            patternData.FileName = filName;

            string patternJson = JsonUtility.ToJson(patternData, true);
            await FileStorage.CreateFile(FolderName, filName, patternJson);
            await UpdateManifestFile();
        }

        /// <summary>
        /// パターンを保存
        /// </summary>
        public async UniTask SavePattern(PatternJsonData patternData)
        {
            string patternJson = JsonUtility.ToJson(patternData, true);
            if (!await FileStorage.UpdateFile(FolderName, patternData.FileName, patternJson))
            {
                Debug.LogError($"パターンセーブ失敗 {patternData.PatternName} {patternData.FileName}");
            }
        }

        /// <summary>
        /// パターン削除
        /// </summary>
        public async UniTask DeletePattern(PatternJsonData patternData)
        {
            await FileStorage.DeleteFile(FolderName,patternData.FileName);
            _manifestData.FileName = _manifestData.FileName.Where(x => x != patternData.FileName).ToArray();
        }

        private async UniTask UpdateManifestFile()
        {
            string manifestJson = JsonUtility.ToJson(_manifestData, true);
            if (!await FileStorage.UpdateFile(FolderName, ManifestFileName, manifestJson))
            {
                Debug.LogError("manifest更新失敗");
                return;
            }
        }

        private async UniTask<string> CreateDefaultManifest()
        {
            var manifestData = new ManifestData();

            string fileName = $"song_{0:D4}.json";

            if (!await FileStorage.TryGetText(FolderName, fileName, null))
            {
                PatternJsonData patternJsonData = _patternLoader.GetDefaultPattern();
                patternJsonData.IsSelect = true;
                patternJsonData.FileName = fileName;
                patternJsonData.IsDefault = true;
                string patternJson = JsonUtility.ToJson(patternJsonData, true);
                await FileStorage.CreateFile(FolderName, fileName, patternJson);
            }

            manifestData.FileName = new string[1];
            manifestData.FileName[0] = fileName;

            return JsonUtility.ToJson(manifestData, true);
        }

        [System.Serializable]
        public class ManifestData
        {
            public string[] FileName;
        }
    }
}