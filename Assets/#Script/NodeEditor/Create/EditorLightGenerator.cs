using System.Collections.Generic;
using InGame.Stage;
using UnityEngine;

public class EditorLightGenerator : EditorGeneratorBase
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform[] _lean;

    private readonly List<EditorLightNode> _clonedNode;
    protected override void UpdateInRange(double minTime, double maxTime)
    {
        List<EditorLightNode> removeNode = new();

        foreach (var node in _clonedNode)
        {
            if (node.Time < minTime || node.Time > maxTime)
            {
                node.Release();
                removeNode.Add(node);
            }
        }

        foreach (var remove in removeNode)
        {
            _clonedNode.Remove(remove);
        }

        foreach(var data in EditorLightData.I.LightData)
        {
            NodeRendering(data, PoolPrefabType.EditorNote);
        }


        void NodeRendering(LightPatternBaseData data, PoolPrefabType editorNote)
        {
            if (data.Time < minTime || data.Time > maxTime)
                return;
            //if (_clonedNode.ContainsKey(node)) return;

            var newNode = PoolManager.I.Get<EditorLightNode>(editorNote, _content);

            newNode.LeanY = _lean[data.Channel].anchoredPosition.y;
            newNode.Time = data.Time;

            //newNode.Data = node;
            //newNode.gameObject.name = $"NormalNode {node.NodeID}";

            switch (data.GetType().Name)
            {
                case nameof(LightPatternBaseData):
                    break;
            }

            _clonedNode.Add(newNode);
        }
    }
}
