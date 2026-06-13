using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        FileLoad();
    }

    private async void FileLoad()
    {
        var data = await NodeDataSerializer.AutoDeserialize(_editFile);
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
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel(
           "Open Save Data",
           "",
           "json");

        if (string.IsNullOrEmpty(path))
            return;

        string fileText = File.ReadAllText(path);

        FileLoad();
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
        File.WriteAllText(path, NodeDataSerializer.SerializeJson(FinalizeNodes(_nodes),EditorManager.I.BPM,EditorManager.I.SongIndex));
#endif
    }
    public List<NodeData> FinalizeNodes(List<NodeData> nodes)
    {
        List<NodeData> result = nodes.OrderBy(x => x.Time).ToList();

        // NodeID振り直し
        for (int i = 0; i < result.Count; i++)
        {
            var node = result[i];
            node.NodeID = i;
            result[i] = node;
        }

        // Hold接続
        for (int i = 0; i < result.Count; i++)
        {
            var startNode = result[i];

            if (startNode.PrefabType != PoolPrefabType.HoldNoteStart)
                continue;

            for (int j = i + 1; j < result.Count; j++)
            {
                var targetNode = result[j];

                // 他レーンは無視
                if (targetNode.Lane != startNode.Lane)
                    continue;

                // 同レーンの終点発見
                if (targetNode.PrefabType == PoolPrefabType.HoldNoteEnd)
                {
                    startNode.Connect = targetNode.NodeID;

                    targetNode.Connect = startNode.NodeID;
                    result[j] = targetNode;

                    break;
                }

                // 同レーンに別ノーツがあったら接続失敗
                break;
            }

            result[i] = startNode;
        }

        return result;
    }
}
