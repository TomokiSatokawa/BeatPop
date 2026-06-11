using System;
using System.Collections.Generic;
using System.IO;
using InGame.Node;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorNodeData : SingletonMonoBehaviour<EditorNodeData>
{
    [SerializeField] private TextAsset _editFile;
    [SerializeField] private Button _exportButton;
    [SerializeField] private Button _importButon;
    public List<NodeData> Nodes => _nodes;
    private List<NodeData> _nodes = new();
    private Subject<NodeData> _onRemove = new();
    public Observable<NodeData> OnRemove => _onRemove;
    private readonly double _epsilon = 0.0001;
    public void Start()
    {
        var data = NodeDataSerializer.AutoDeserialize(_editFile);
        _nodes = data.Nodes;
        EditorManager.I.SetData(data.BPM, data.SoundIndex);
        _importButon.onClick.AddListener(OnImport);
        _exportButton.onClick.AddListener(OnExport);
    }
    
    public void AddNode(PoolPrefabType prefab,double time,int lean)
    {
        if (_nodes.Exists(x => Math.Abs(x.Time - time) < _epsilon && x.Lane == lean)) return;

        _nodes.Add(new NodeData()
        {
            Time = (float)time,
            Lane = lean,
            PrefabType = prefab
        });
    }
    public void DeleteNode(double time , int lean)
    {
        var targetNode = _nodes.FindIndex(x => Math.Abs(x.Time - time) < _epsilon && x.Lane == lean);
        if(targetNode == -1) return;

        _onRemove.OnNext(_nodes[targetNode]);
        _nodes.RemoveAt(targetNode);
    }
    public void OnImport()
    {
        string path = EditorUtility.OpenFilePanel(
           "Open Save Data",
           "",
           "json");

        if (string.IsNullOrEmpty(path))
            return;

        string fileText = File.ReadAllText(path);

        var data = NodeDataSerializer.AutoDeserialize(fileText,path);
        _nodes = data.Nodes;
        EditorManager.I.SetData(data.BPM, data.SoundIndex);
    }
    public void OnExport()
    {
        string path = EditorUtility.SaveFilePanel(
            "Save Save Data",
            "",
            "SaveData",
            "json");

        if (string.IsNullOrEmpty(path))
            return;
        File.WriteAllText(path, NodeDataSerializer.SerializeJson(_nodes,EditorManager.I.BPM,EditorManager.I.SongIndex));
    }
}
