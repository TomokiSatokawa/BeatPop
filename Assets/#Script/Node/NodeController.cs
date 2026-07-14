using System.Collections.Generic;
using System.Linq;
using Common.BeatUpdate;
using InGame.UI;
using Input;
using R3;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// NodeGeneratorÇ≈ê∂ê¨ÇµÇΩÉmÅ[ÉcÇìÆÇ©Ç∑
    /// </summary>
    public class NodeController : MonoBehaviour
    {
        [SerializeField] private NodeHitExecutor _nodeHitExecutor;
        private List<NodeObject> _nodes = new();

        private readonly List<NodeObject> _removeNodes = new();

        public void AddNode(NodeObject node)
        {
            _nodes.Add(node);
        }

        public void Update()
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return;

            _removeNodes.Clear();
            foreach (NodeObject node in _nodes)
            {
                float deleteTime = StageTimeController.StageTime - JudgementManager.I.DeleteTime;

                if (node.NodeData.Time <= deleteTime)
                {
                    _removeNodes.Add(node);
                }
                float startTime = node.NodeData.Time - NodeGenerator.I.ArrivalSeconds;

                float progress = (StageTimeController.StageTime - startTime) / (node.NodeData.Time - startTime);

                Vector3 startPosition = node.StartPosition;
                Vector3 endPosition = startPosition;
                endPosition.z = node.GoalPos;
                node.transform.position = Vector3.LerpUnclamped(startPosition, endPosition, progress);
            }

            foreach (NodeObject node in _removeNodes)
            {
                if (node.Type != PoolPrefabType.Line)
                {
                 _nodeHitExecutor.RemoveAction(node);
                }
                node.Release();
                _nodes.Remove(node);
            }
        }
        public NodeObject GetClickNode(int lane)
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return null;

            NodeObject targetNode = null;
            float bestDifference = float.MaxValue;
            float nodeTime = 0;

            foreach (var node in _nodes)
            {
                if (node.Type == PoolPrefabType.Line
                    || node.NodeData.Lane != lane) continue;

                float difference =
                    Mathf.Abs(node.NodeData.Time - StageTimeController.StageTime);

                if (difference < bestDifference)
                {
                    bestDifference = difference;
                    targetNode = node;
                    nodeTime = node.NodeData.Time;
                }
            }

            if (targetNode == null) return null;

            if (bestDifference <= JudgementManager.I.ToleranceValue)
            {
                return targetNode;
            }
            return null;
        }

        public void ClickNode(NodeObject targetNode)
        {
            targetNode.Release();
            _nodes.Remove(targetNode);
            _nodeHitExecutor.HitAction(targetNode);
        }

        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.Where(x => x.NodeData.NodeID == nodeID).FirstOrDefault();
        }
    }
}