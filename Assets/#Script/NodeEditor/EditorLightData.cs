using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using InGame.Stage;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorLightData : SingletonMonoBehaviour<EditorLightData>
{
    [SerializeField] private Button _exportButton;
    [SerializeField] private Button _importButton;
    [SerializeField] private Button _newFileButton;

    private List<LightPatternBaseData> _lightData = new();
    public IReadOnlyList<LightPatternBaseData> LightData => _lightData;

    private readonly ReactiveProperty<StageSaveData> _loadedFile = new();
    public ReadOnlyReactiveProperty<StageSaveData> LoadedFile => _loadedFile;
    private readonly double _epsilon = 0.0001;

    public void Start()
    {
        _importButton.onClick.AddListener(OnImport);
        _exportButton.onClick.AddListener(OnExport);
        _newFileButton.onClick.AddListener(CreateNewFile);
    }

    private  void FileLoad(string file)
    {
        var data = StageDataSerializer.DeserializeJson(file);
        if (data == null)
        {
            Debug.LogError("ファイル読み込みエラー");
            return;
        }
        _lightData = data.LightData.ToList();
        _loadedFile.OnNext(data);
    }

    public void AddNode(LightPatternBaseData lightData, float time, int channel)
    {
        if (_lightData.Exists(x => Mathf.Abs(x.Time - time) < _epsilon && x.Channel == channel)) return;

        _lightData.Add(lightData);
    }
    public void DeleteNode(float time, int channel)
    {
        var targetNode = _lightData.FindIndex(x => Mathf.Abs(x.Time - time) < _epsilon && x.Channel == channel);
        if (targetNode == -1) return;

        //_onRemove.OnNext(_nodes[targetNode]);
        _lightData.RemoveAt(targetNode);
    }
    public void OnImport()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel(
           "Open Save Data",
           "",
           "json");

        if (string.IsNullOrEmpty(path))
            return;

        string fileText = File.ReadAllText(path);

        FileLoad(fileText);
#endif
    }
    public void OnExport()
    {
#if UNITY_EDITOR
        string path = EditorUtility.SaveFilePanel(
            "Save Save Data",
            "",
            "SaveData",
            "json");

        if (string.IsNullOrEmpty(path))
            return;

        string json = StageDataSerializer.SerializeJson(_lightData.ToArray());
        File.WriteAllText(path, json);
#endif
    }
    public void CreateNewFile()
    {
        _loadedFile.Value = new();
    }
}
