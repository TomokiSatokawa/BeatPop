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
        public static readonly int MaxPatternCount = 10;

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
//#if UNITY_IOS && !UNITY_EDITOR
//            // iOSの場合はメモリ上にデフォルトパターンのみを生成
//            _inMemoryPatterns.Clear();
//            var defaultPattern = _patternLoader.GetDefaultPattern();
//            defaultPattern.IsSelect = true;
//            defaultPattern.FileName = "song_0000.json";
//            defaultPattern.IsDefault = true;
//            _inMemoryPatterns.Add(defaultPattern);

//            _manifestData = new ManifestData
//            {
//                FileName = new string[] { defaultPattern.FileName }
//            };

//            await UniTask.CompletedTask;
//#else
            string manifestJson = "";

            if (!await FileStorage.TryGetText(FolderName, ManifestFileName, t => manifestJson = t))
            {
                manifestJson = await CreateDefaultManifest();
                FileStorage.CreateFile(FolderName, ManifestFileName, manifestJson);
            }

            _manifestData = JsonUtility.FromJson<ManifestData>(manifestJson);

            foreach (var fileIndex in _manifestData.FileIndexes)
            {
                string fileName = GetFileName(fileIndex);
                if (!await FileStorage.TryGetText(FolderName, fileName, null))
                {
                    Debug.LogError($"ファイル破損 {fileName}");
                }

                await UniTask.Yield();
            }
//#endif
        }

        public async UniTask<PatternJsonData[]> GetAllCustomPattern()
        {
//#if UNITY_IOS && !UNITY_EDITOR
//            await UniTask.CompletedTask;
//            return _inMemoryPatterns.ToArray();
//#else
            var result = new PatternJsonData[_manifestData.FileIndexes.Length];
            for (int i = 0; i < _manifestData.FileIndexes.Length; i++)
            {
                string patternJson = "";
                int fileIndex = _manifestData.FileIndexes[i];
                if (!await FileStorage.TryGetText(FolderName, GetFileName(fileIndex), t => patternJson = t))
                {
                    Debug.LogError($"{fileIndex}");
                    continue;
                }
                result[i] = JsonUtility.FromJson<PatternJsonData>(patternJson);
            }
            return result;
//#endif
        }

        public bool AddPattern(PatternJsonData patternData)
        {
            //#if UNITY_IOS && !UNITY_EDITOR
            //            string fileName = $"song_{_inMemoryPatterns.Count:D4}.json";
            //            patternData.FileName = fileName;
            //            _inMemoryPatterns.Add(patternData);

            //            Array.Resize(ref _manifestData.FileName, _manifestData.FileName.Length + 1);
            //            _manifestData.FileName[^1] = fileName;

            //            await UniTask.CompletedTask;
            //#else

            if (_manifestData.FileIndexes.Length >= MaxPatternCount)
                return false;

            Array.Resize(ref _manifestData.FileIndexes, _manifestData.FileIndexes.Length + 1);


            int newIndex = -1;
            for (int i = 0; i < 10; i++)
            {
                if (_manifestData.FileIndexes.Contains(i)) continue;
                newIndex = i;
                break;
            }

            if (newIndex == -1)
                return false;

                _manifestData.FileIndexes[_manifestData.FileIndexes.Length - 1] = newIndex;
            patternData.FileIndex = newIndex;

            string patternJson = JsonUtility.ToJson(patternData, true);
            FileStorage.CreateFile(FolderName, GetFileName(newIndex), patternJson);
            UpdateManifestFile();

            return true;
//#endif
        }

        public void SavePattern(PatternJsonData patternData)
        {
//#if UNITY_IOS && !UNITY_EDITOR
//            int index = _inMemoryPatterns.FindIndex(x => x.FileName == patternData.FileName);
//            if (index >= 0)
//            {
//                _inMemoryPatterns[index] = patternData;
//            }
//            return;
//#else
            string patternJson = JsonUtility.ToJson(patternData, true);
            FileStorage.UpdateFile(FolderName, GetFileName(patternData.FileIndex), patternJson);
//#endif
        }

        public void DeletePattern(PatternJsonData patternData)
        {
            //#if UNITY_IOS && !UNITY_EDITOR
            //            _inMemoryPatterns.RemoveAll(x => x.FileName == patternData.FileName);
            //            _manifestData.FileName = _manifestData.FileName.Where(x => x != patternData.FileName).ToArray();
            //           return;
            //#else

            string fileName = GetFileName(patternData.FileIndex);

            FileStorage.DeleteFile(FolderName, fileName);
            _manifestData.FileIndexes = _manifestData.FileIndexes.Where(x => x != patternData.FileIndex).ToArray();
//#endif
        }

        private void UpdateManifestFile()
        {
//#if UNITY_IOS && !UNITY_EDITOR
//           return;
//#else
            string manifestJson = JsonUtility.ToJson(_manifestData, true);
            FileStorage.UpdateFile(FolderName, ManifestFileName, manifestJson);
//#endif
        }

        private async UniTask<string> CreateDefaultManifest()
        {
            var manifestData = new ManifestData();

            int fileIndex = 0;
            string fileName = GetFileName(fileIndex);

            if (!await FileStorage.TryGetText(FolderName, fileName, null))
            {
                PatternJsonData patternJsonData = _patternLoader.GetDefaultPattern();
                patternJsonData.IsSelect = true;
                patternJsonData.FileIndex = fileIndex;
                patternJsonData.IsDefault = true;
                string patternJson = JsonUtility.ToJson(patternJsonData, true);
                FileStorage.CreateFile(FolderName, fileName, patternJson);
            }

            manifestData.FileIndexes = new int[1];
            manifestData.FileIndexes[0] = fileIndex;

            return JsonUtility.ToJson(manifestData, true);
        }

        [System.Serializable]
        public class ManifestData
        {
            public int[] FileIndexes;
        }

        private static string GetFileName(int index)
        {
            return $"song_{index:D4}.json";
        }
    }
}