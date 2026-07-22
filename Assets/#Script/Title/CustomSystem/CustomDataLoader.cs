using System;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CustomDataLoader : SingletonMonoBehaviour<CustomDataLoader>
{
    [SerializeField] private CustomPatternLoader _patternLoader;
    [SerializeField] private SongListDataBase _songData;

    private ManifestData _manifestData;
    private const string MANIFEST_FILE_NAME = "manifest.json";

    public async UniTask LoadManifest(CancellationToken cancellationToken)
    {
        string manifestJson = "";

        //manifestを作る
        if (!await CustomPatternFile.TryGetText(MANIFEST_FILE_NAME, t => manifestJson = t))
        {
            manifestJson = await CreateDefaultManifest();
            await CustomPatternFile.CreateFile(MANIFEST_FILE_NAME, manifestJson);
        }

        _manifestData = JsonUtility.FromJson<ManifestData>(manifestJson);

        //SongData=>manifestの照合

        foreach (var fillPath in _manifestData.FileName)
        {
            if (!await CustomPatternFile.TryGetText(fillPath, null))
            {
                Debug.LogError($"ファイル破損 {fillPath}");
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public async UniTask<PatternJsonData[]> GetCustomPattern()
    {
        var result = new PatternJsonData[_manifestData.FileName.Length];
        for (int i = 0; i < _manifestData.FileName.Length; i++)
        {
            string patternJson = "";
            string fileName = _manifestData.FileName[i];
            if (!await CustomPatternFile.TryGetText(fileName, t => patternJson = t))
            {
                Debug.LogError($"{fileName}");
                continue;
            }
            result[i] = JsonUtility.FromJson<PatternJsonData>(patternJson);
        }
        return result;
    }
    public async void AddPattern(PatternJsonData patternData)
    {
        Array.Resize(ref _manifestData.FileName, _manifestData.FileName.Length + 1);

        string filName = $"song_{(_manifestData.FileName.Length - 1):D4}.json";
        _manifestData.FileName[_manifestData.FileName.Length - 1] = filName;
        patternData.FillName = filName;

        string patternJson = JsonUtility.ToJson(patternData, true);
        await CustomPatternFile.CreateFile(filName, patternJson);
        await UpdateManifestFill();
    }

    public async UniTask SavePattern(PatternJsonData patternData)
    {
        string patternJson = JsonUtility.ToJson(patternData, true);
        if (!await CustomPatternFile.UpdateFile(patternData.FillName, patternJson))
        {
            Debug.LogError($"パターンセーブ失敗 {patternData.PatternName} {patternData.FillName}");
        }
    }

    private async UniTask UpdateManifestFill()
    {
        string manifestJosn = JsonUtility.ToJson(_manifestData, true);
        if (!await CustomPatternFile.UpdateFile(MANIFEST_FILE_NAME, manifestJosn))
        {
            Debug.LogError("manifest更新失敗");
            return;
        }
    }

    private async UniTask<string> CreateDefaultManifest()
    {
        var manifestData = new ManifestData();

        string filName = $"song_{0:D4}.json";

        if (!await CustomPatternFile.TryGetText(filName, null))
        {
            PatternJsonData patternJsonData = _patternLoader.GetDefaultPattern();
            patternJsonData.IsSelect = true;
            patternJsonData.FillName = filName;
            string patternJson = JsonUtility.ToJson(patternJsonData, true);
            await CustomPatternFile.CreateFile(filName, patternJson);
        }

        manifestData.FileName = new string[1];
        manifestData.FileName[0] = filName;

        return GetManifestJson(manifestData);
    }
    private string GetManifestJson(ManifestData songDatas)
    {
        string json = JsonUtility.ToJson(songDatas, true);
        return json;
    }

    [System.Serializable]
    public class ManifestData
    {
        public string[] FileName;
    }
}
