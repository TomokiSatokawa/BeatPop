using System;
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
    private Subject<(float time, int channel)> _onRemove = new();
    public Observable<(float time, int channel)> OnRemove => _onRemove;
    private readonly double _epsilon = 0.0001;

    public void Start()
    {
        _importButton.onClick.AddListener(OnImport);
        _exportButton.onClick.AddListener(OnExport);
        _newFileButton.onClick.AddListener(CreateNewFile);
    }

    private void FileLoad(string file)
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

    public void AddNode(LightPatternBaseData lightData)
    {
        if (_lightData.Exists(x => Mathf.Abs(x.Time - lightData.Time) < _epsilon && x.Channel == lightData.Channel)) return;

        _lightData.Add(lightData);
        _lightData = _lightData.OrderBy(x => x.Time).ToList();
    }
    public void DeleteNode(float time, int channel)
    {
        var targetNode = _lightData.FindIndex(x => Mathf.Abs(x.Time - time) < _epsilon && x.Channel == channel);
        if (targetNode == -1) return;

        _onRemove.OnNext((time, channel));
        _lightData.RemoveAt(targetNode);
        _lightData = _lightData.OrderBy(x => x.Time).ToList();
    }
    public LightPatternBaseData ChangeType(LightPatternBaseData lightData, Type type)
    {
        if (lightData.GetType() == type) return lightData;

        var targetIndex = _lightData.IndexOf(lightData);
        if (targetIndex == -1) return null;

        if (!typeof(LightPatternBaseData).IsAssignableFrom(type))
        {
            Debug.LogError($"{type.Name} は LightPatternBaseData を継承していません");
            return null;
        }

        var newData = (LightPatternBaseData)Activator.CreateInstance(type);

        newData.PatternType = lightData.PatternType;
        newData.Time = lightData.Time;
        newData.Channel = lightData.Channel;
        newData.Duration = lightData.Duration;
        newData.Power = lightData.Power;
        newData.MainColor = lightData.MainColor;

        _lightData[targetIndex] = newData;
        return newData;
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

        string json = StageDataSerializer.SerializeJson(_lightData.ToArray(), LoadedFile.CurrentValue.SongDataIndex);
        File.WriteAllText(path, json);
#endif
    }
    public void CreateNewFile()
    {
        _loadedFile.Value = new();
    }
}
