using System;
using System.Linq;
using System.Threading;
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

        foreach (var manifestSongData in _manifestData.Datas)
        {
            foreach (var fillPath in manifestSongData.FileName)
            {
                if (!await CustomPatternFile.TryGetText(fillPath, null))
                {
                    Debug.LogError($"ファイル破損 {fillPath}");
                }
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public async UniTask<PatternJsonData[]> GetCustomPattern(int songId)
    {
        var manifestSongData = _manifestData.Datas.Where(x => x.SongID == songId).FirstOrDefault();
        if (manifestSongData == null)
        {
            Debug.LogError("Song Pattern not found");
            return null;
        }

        var result = new PatternJsonData[manifestSongData.FileName.Length];
        for (int i = 0; i < manifestSongData.FileName.Length; i++)
        {
            string patternJson = "";
            string fileName = manifestSongData.FileName[i];
            if (!await CustomPatternFile.TryGetText(fileName, t => patternJson = t))
            {
                Debug.LogError($"{fileName}");
                continue;
            }
            result[i] = JsonUtility.FromJson<PatternJsonData>(patternJson);
        }
        return result;
    }
    public async void AddPattern(int songId, PatternJsonData patternData)
    {
        var manifestSongData = _manifestData.Datas.Where(x => x.SongID == songId).FirstOrDefault();
        Array.Resize(ref manifestSongData.FileName, manifestSongData.FileName.Length + 1);

        string filName = $"song_{songId:D4}_{(manifestSongData.FileName.Length - 1):D4}.json";
        manifestSongData.FileName[manifestSongData.FileName.Length - 1] = filName;
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
        var manifestSongDatas = new ManifestSongData[_songData.SongDates.Count];
        for (int i = 0; i < _songData.SongDates.Count; i++)
        {
            int songID = _songData.SongDates[i].SongID;
            string filName = $"song_{songID:D4}_{0:D4}.json";

            if (!await CustomPatternFile.TryGetText(filName, null))
            {
                PatternJsonData patternJsonData = _patternLoader.GetDefaultPattern();
                patternJsonData.IsSelect = true;
                patternJsonData.FillName = filName;
                string patternJson = JsonUtility.ToJson(patternJsonData, true);
                await CustomPatternFile.CreateFile(filName, patternJson);
            }
            manifestSongDatas[i] = new();
            manifestSongDatas[i].FileName = new string[] { filName };
            manifestSongDatas[i].SongID = _songData.SongDates[i].SongID;
        }
        return GetManifestJson(manifestSongDatas);
    }
    private string GetManifestJson(ManifestSongData[] songDatas)
    {
        string json = JsonUtility.ToJson(new ManifestData { Datas = songDatas }, true);
        return json;
    }

    [System.Serializable]
    public class ManifestData
    {
        public ManifestSongData[] Datas;
    }
    [System.Serializable]
    public class ManifestSongData
    {
        public string[] FileName;
        public int SongID;
    }
}
