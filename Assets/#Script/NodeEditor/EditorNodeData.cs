using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InGame.Node;
using InGame.UI;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorNodeData : SingletonMonoBehaviour<EditorNodeData>
{
    [SerializeField] private TextAsset _editFile;
    [SerializeField] private Button _exportButton;
    [SerializeField] private Button _importButton;
    public List<NodeData> Nodes => _nodes;
    private List<NodeData> _nodes = new();
    private List<float> _sectionTime = new();
    public IReadOnlyList<float> SectionTime => _sectionTime;
    private Subject<NodeData> _onRemove = new();
    public Observable<NodeData> OnRemove => _onRemove;

    private ReactiveProperty<NodeSaveData> _loadedFile = new();
    public ReadOnlyReactiveProperty<NodeSaveData> LoadedFile => _loadedFile;

    private readonly double _epsilon = 0.0001;


    public void Start()
    {
        _importButton.onClick.AddListener(OnImport);
        _exportButton.onClick.AddListener(OnExport);
    }

    private async void FileLoad(string file)
    {
        var data = await NodeDataSerializer.AutoDeserialize(file);
        if (data != null)
        {
            _nodes = data.Nodes;
            _loadedFile.OnNext(data);
        }
    }

    public void AddNode(PoolPrefabType prefab, double time, int lean)
    {
        if (_nodes.Exists(x => Math.Abs(x.Time - time) < _epsilon && x.Lane == lean)) return;

        _nodes.Add(new NodeData()
        {
            Time = (float)time,
            Lane = lean,
            PrefabType = prefab
        });
    }
    public void DeleteNode(double time, int lean)
    {
        var targetNode = _nodes.FindIndex(x => Math.Abs(x.Time - time) < _epsilon && x.Lane == lean);
        if (targetNode == -1) return;

        _onRemove.OnNext(_nodes[targetNode]);
        _nodes.RemoveAt(targetNode);
    }

    public void AddSection(float time)
    {
        float tolerance = 0.0001f;
        if (_sectionTime.Exists(t => Mathf.Abs(t - time) < tolerance))
            return;

        _sectionTime.Add(time);
    }

    public void RemoveSection(float time)
    {
        float tolerance = 0.0001f;
        int index = _sectionTime.FindIndex(t => Mathf.Abs(t - time) < tolerance);
        Debug.Log("Re");
        if (index == -1)
            return;

        Debug.Log("Remove");
        _sectionTime.RemoveAt(index);
    }

    public void OnImport()
    {
        Debug.Log("a");
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

        File.WriteAllText(path, NodeDataSerializer.SerializeJson(
            FinalizeNodes(_nodes), FinalizeSection(_sectionTime),
            StageTimeController.I.BPM, LoadedFile.CurrentValue.SoundIndex));
#endif
    }
    private List<NodeData> FinalizeNodes(List<NodeData> nodes)
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

    private List<float> FinalizeSection(List<float> section)
    {
        if (!section.Contains(0f))
        {
            section.Add(0f);
        }

        return section.OrderBy(x => x).ToList();
    }
}
