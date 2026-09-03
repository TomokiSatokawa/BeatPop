using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Title.Custom
{
    public class CustomDataLoader : SingletonPersistent<CustomDataLoader>
    {
        [SerializeField] private CustomPatternLoader _patternLoader;
        [SerializeField] private bool _isAutoLoad;

        private ManifestData _manifestData;
        private const string ManifestFileName = "manifest.json";
        private const string FolderName = "CustomData";

        // iOS用のインメモリ保存キャッシュ
        private readonly List<PatternJsonData> _inMemoryPatterns = new();

        protected override void OnAwake()
        {
            if (_isAutoLoad)
            {
                LoadManifest().Forget();
            }
        }

        public async UniTask LoadManifest()
        {
#if UNITY_IOS && !UNITY_EDITOR
            // iOSの場合はメモリ上にデフォルトパターンのみを生成
            _inMemoryPatterns.Clear();
            var defaultPattern = _patternLoader.GetDefaultPattern();
            defaultPattern.IsSelect = true;
            defaultPattern.FileName = "song_0000.json";
            defaultPattern.IsDefault = true;
            _inMemoryPatterns.Add(defaultPattern);

            _manifestData = new ManifestData
            {
                FileName = new string[] { defaultPattern.FileName }
            };

            await UniTask.CompletedTask;
#else
            string manifestJson = "";

            if (!await FileStorage.TryGetText(FolderName, ManifestFileName, t => manifestJson = t))
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
#endif
        }

        public async UniTask<PatternJsonData[]> GetAllCustomPattern()
        {
#if UNITY_IOS && !UNITY_EDITOR
            await UniTask.CompletedTask;
            return _inMemoryPatterns.ToArray();
#else
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
#endif
        }

        public async UniTask AddPattern(PatternJsonData patternData)
        {
#if UNITY_IOS && !UNITY_EDITOR
            string fileName = $"song_{_inMemoryPatterns.Count:D4}.json";
            patternData.FileName = fileName;
            _inMemoryPatterns.Add(patternData);

            Array.Resize(ref _manifestData.FileName, _manifestData.FileName.Length + 1);
            _manifestData.FileName[^1] = fileName;

            await UniTask.CompletedTask;
#else
            Array.Resize(ref _manifestData.FileName, _manifestData.FileName.Length + 1);

            string filName = $"song_{(_manifestData.FileName.Length - 1):D4}.json";
            _manifestData.FileName[_manifestData.FileName.Length - 1] = filName;
            patternData.FileName = filName;

            string patternJson = JsonUtility.ToJson(patternData, true);
            await FileStorage.CreateFile(FolderName, filName, patternJson);
            await UpdateManifestFile();
#endif
        }

        public async UniTask SavePattern(PatternJsonData patternData)
        {
#if UNITY_IOS && !UNITY_EDITOR
            int index = _inMemoryPatterns.FindIndex(x => x.FileName == patternData.FileName);
            if (index >= 0)
            {
                _inMemoryPatterns[index] = patternData;
            }
            await UniTask.CompletedTask;
#else
            string patternJson = JsonUtility.ToJson(patternData, true);
            if (!await FileStorage.UpdateFile(FolderName, patternData.FileName, patternJson))
            {
                Debug.LogError($"パターンセーブ失敗 {patternData.PatternName} {patternData.FileName}");
            }
#endif
        }

        public async UniTask DeletePattern(PatternJsonData patternData)
        {
#if UNITY_IOS && !UNITY_EDITOR
            _inMemoryPatterns.RemoveAll(x => x.FileName == patternData.FileName);
            _manifestData.FileName = _manifestData.FileName.Where(x => x != patternData.FileName).ToArray();
            await UniTask.CompletedTask;
#else
            await FileStorage.DeleteFile(FolderName, patternData.FileName);
            _manifestData.FileName = _manifestData.FileName.Where(x => x != patternData.FileName).ToArray();
#endif
        }

        private async UniTask UpdateManifestFile()
        {
#if UNITY_IOS && !UNITY_EDITOR
            await UniTask.CompletedTask;
#else
            string manifestJson = JsonUtility.ToJson(_manifestData, true);
            if (!await FileStorage.UpdateFile(FolderName, ManifestFileName, manifestJson))
            {
                Debug.LogError("manifest更新失敗");
                return;
            }
#endif
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