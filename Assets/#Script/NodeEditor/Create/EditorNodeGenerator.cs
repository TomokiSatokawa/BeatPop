using System.Collections.Generic;
using InGame.Node;
using R3;
using UnityEngine;

public class EditorNodeGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _extraClone;
    [SerializeField] private RectTransform[] _lean;

    private readonly Dictionary<NodeData, FollowTime> _clonedNode = new();
    public void Start()
    {
        EditorNodeData.I.OnRemove.Subscribe(x => RemoveNode(x)).AddTo(this);
    }
    private void Update()
    {
        double extraTime = _extraClone / EditorManager.I.Magnification;

        double displayTime =
            EditorManager.I.DisplayRange / EditorManager.I.Magnification;

        double minTime = EditorManager.I.EditorTime.CurrentValue - extraTime;

        double maxTime = EditorManager.I.EditorTime.CurrentValue + displayTime + extraTime;


        List<NodeData> removeNode = new();

        foreach (var node in _clonedNode.Keys)
        {
            if (node.Time < minTime || node.Time > maxTime)
            {
                _clonedNode[node].Release();
                removeNode.Add(node);
            }
        }

        foreach (var remove in removeNode)
        {
            _clonedNode.Remove(remove);
        }

        foreach (var node in EditorNodeData.I.Nodes)
        {
            if (node.Time < minTime || node.Time > maxTime)
                continue;
            if (_clonedNode.ContainsKey(node)) continue;

            var newNode = PoolManager.I.Get<EditorNode>(PoolPrefabType.EditorNote, _content);

            newNode.LeanY = _lean[node.Lane].anchoredPosition.y;
            newNode.Time = node.Time;
            newNode.Data = node;
            newNode.gameObject.name = $"NomalNode {node.NodeID}";

            switch (node.PrefabType)
            {
                case PoolPrefabType.NormalNote:
                    newNode.ChangeColor(Color.white); 
                    break;
                case PoolPrefabType.FlickNote:
                    newNode.ChangeColor(Color.yellow);
                    break;
                case PoolPrefabType.HoldNoteStart:
                    newNode.ChangeColor(Color.green);
                    break;
                case PoolPrefabType.HoldNoteFill:
                    newNode.ChangeColor(Color.lightGreen);
                    break;
                case PoolPrefabType.HoldNoteEnd:
                    newNode.ChangeColor(Color.darkGreen);
                    break;
            }

            _clonedNode.Add(node, newNode);
            EditorManager.I.ReloadTime();
        }
    }
    public void RemoveNode(NodeData node)
    {
        Debug.Log(node + "Remove");
        FollowTime poolObject = _clonedNode[node];
        poolObject.Release();
        _clonedNode.Remove(node);

    }
}