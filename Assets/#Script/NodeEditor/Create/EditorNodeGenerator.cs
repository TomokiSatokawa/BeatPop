using System.Collections.Generic;
using InGame.Node;
using R3;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// ÉmÅ[ÉcÇÃï\é¶
    /// </summary>
    public class EditorNodeGenerator : EditorGeneratorBase<EditorNodeGenerator>
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform[] _lean;

        private readonly Dictionary<NodeData, FollowTime> _clonedNode = new();

        private void Start()
        {
            EditorNodeData.I.OnRemove.Subscribe(x => RemoveNode(x)).AddTo(this);
        }

        protected override void UpdateInRange(double minTime, double maxTime)
        {
            List<NodeData> removeNodes = new();

            foreach (var node in _clonedNode.Keys)
            {
                if (node.Time < minTime || node.Time > maxTime)
                {
                    removeNodes.Add(node);
                }
            }

            foreach (var remove in removeNodes)
            {
                if (!_clonedNode.TryGetValue(remove, out var poolObject))
                    continue;

                poolObject.Release();
                _clonedNode.Remove(remove);
            }

            foreach (var node in EditorNodeData.I.Nodes)
            {
                RenderNode(minTime, maxTime, node, PoolPrefabType.EditorNote);
            }

            foreach (var section in EditorNodeData.I.SectionTime)
            {
                var node = new NodeData()
                {
                    Time = section,
                    Lane = 0,
                    PrefabType = PoolPrefabType.SectionNode
                };

                RenderNode(minTime, maxTime, node, PoolPrefabType.SectionNode);
            }
        }

        private void RenderNode(double minTime, double maxTime, NodeData node, PoolPrefabType editorNote)
        {
            if (node.Time < minTime || node.Time > maxTime)
                return;
            if (_clonedNode.ContainsKey(node)) return;

            var newNode = CreateNode(node, editorNote);

            ApplyNodeColor(node, newNode);

            _clonedNode.Add(node, newNode);
        }

        private void ApplyNodeColor(NodeData node, EditorNode newNode)
        {
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
                case PoolPrefabType.SectionNode:
                    newNode.ChangeColor(Color.gray);
                    break;
            }
        }

        private EditorNode CreateNode(NodeData node, PoolPrefabType editorNote)
        {
            var newNode = PoolManager.I.Get<EditorNode>(editorNote, _content);

            newNode.LeanY = _lean[node.Lane].anchoredPosition.y;
            newNode.Time = node.Time;
            newNode.Data = node;
            newNode.gameObject.name = $"{node.PrefabType} {node.NodeID}";
            return newNode;
        }

        public void RemoveNode(NodeData node)
        {
            if (!_clonedNode.TryGetValue(node, out var poolObject))
                return;

            poolObject.Release();
            _clonedNode.Remove(node);
        }
    }
}