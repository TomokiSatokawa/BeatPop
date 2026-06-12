using System.Collections.Generic;
using System.Linq;
using InGame.Node;
using R3;
using Sound;
using UnityEngine;

public class EditorNodeSound : MonoBehaviour
{
    public List<NodeData> _soundTimeData;
    public void Start()
    {
        EditorManager.I.IsPlaying
            .Where(x => x)
            .Subscribe(_ => UpdateNodeData())
            .AddTo(this);
    }
    public void UpdateNodeData()
    {
        _soundTimeData = new();
        foreach (var nodeData in EditorNodeData.I.Nodes)
        {
            if (nodeData.Time < EditorManager.I.EditorTime.CurrentValue)
            {
                continue;
            }
                _soundTimeData.Add(nodeData);
        }
        _soundTimeData = _soundTimeData.OrderBy(x => x.Time).ToList();
    }
    void Update()
    {
        if (!EditorManager.I.IsPlaying.CurrentValue) return;
        List<NodeData> removeList = new();

        foreach (var nodeData in _soundTimeData)
        {
            if (nodeData.Time <= EditorManager.I.EditorTime.CurrentValue)
            {
                SoundManager.I.PlaySESound(SESoundType.NormalTap);
                removeList.Add(nodeData);
                continue;
            }
            break;
        }

        foreach (var nodeData in removeList)
        {
            _soundTimeData.Remove(nodeData);
        }
    }
}