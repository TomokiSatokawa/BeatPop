using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CustomManifestLoader : SingletonMonoBehaviour<CustomManifestLoader>
{
    [SerializeField] private CustomPatternLoader _patternLoader;
    [SerializeField] private SongListDataBase _songData;

    private ManifestData _manifestData;
    private const string MANIFEST_FILE_NAME = "manifest.json";

    public async UniTask LoadManifest(CancellationToken cancellationToken)
    {
        string manifestJson = "";
        if (!await CustomPatternFile.TryGetText(MANIFEST_FILE_NAME, t => manifestJson = t))
        {
            manifestJson = await CreateDefaultManifest();
            await CustomPatternFile.CreateFile(MANIFEST_FILE_NAME, manifestJson);
        }

        _manifestData = JsonUtility.FromJson<ManifestData>(manifestJson);
        foreach (var manifestSongData in _manifestData.Datas)
        {
            foreach(var fillpath in manifestSongData.FileName)
            {
                if(!await CustomPatternFile.TryGetText(fillpath, null))
                {
                    Debug.LogError($"ƒtƒ@ƒCƒ‹”j‘¹ {fillpath}");
                }
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public async UniTask<PatternJsonData[]> GetCustomPattern(int songId)
    {
        var manifestSongData = _manifestData.Datas.Where(x => x.SongID == songId).FirstOrDefault();
        if(manifestSongData == null)
        {
            Debug.LogError("Song Pattern not found");
            return null;
        }

        var result = new PatternJsonData[manifestSongData.FileName.Length];
        for(int i = 0; i < manifestSongData.FileName.Length; i++)
        {
            string patternJson = "";
            string fileName = manifestSongData.FileName[i];
            if (!await CustomPatternFile.TryGetText(fileName,t =>  patternJson = t))
            {
                Debug.LogError($"{fileName}");
                continue;
            }
            result[i] = JsonUtility.FromJson<PatternJsonData>(patternJson);
        }
        return result;
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
                await CustomPatternFile.CreateFile(filName, _patternLoader.GetDefaultPattern());
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
