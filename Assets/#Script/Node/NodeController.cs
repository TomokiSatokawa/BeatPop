using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// NodeGeneratorで生成したノーツを動かす
    /// </summary>
    public class NodeController : MonoBehaviour
    {
        [SerializeField] private JudgementTable _judgementTable;
        [SerializeField] private NodeHitExecutor _nodeHitExecutor;
        [SerializeField] private float _goalPos;

        private readonly List<NodeObject> _nodes = new();
        private readonly List<NodeObject> _removeNodes = new();

        public void AddNode(NodeObject node)
        {
            _nodes.Add(node);
        }

        private void Update()
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return;

            UpdateNodes();
            RemoveExpiredNodes();
        }

        private void UpdateNodes()
        {
            _removeNodes.Clear();

            float stageTime = StageTimeController.StageTime;
            float deleteTime = stageTime - _judgementTable.DeleteTime;

            foreach (NodeObject node in _nodes)
            {

                if (node.NodeData.Time <= deleteTime)
                {
                    _removeNodes.Add(node);
                }
                float startTime = node.NodeData.Time - NodeGenerator.I.ArrivalSeconds;

                float progress = (stageTime - startTime) / (node.NodeData.Time - startTime);

                Vector3 startPosition = node.StartPosition;//TODO:StageSOとNodeObject.NodeData.Laneで取得する
                Vector3 endPosition = startPosition;
                endPosition.z = _goalPos;
                node.transform.position = Vector3.LerpUnclamped(startPosition, endPosition, progress);
            }
        }

        private void RemoveExpiredNodes()
        {
            foreach (NodeObject node in _removeNodes)
            {
                if (node.Type != PoolPrefabType.Line)
                {
                    _nodeHitExecutor.HandleRemove(node);
                }
                node.Release();
                _nodes.Remove(node);
            }
        }

        /// <summary>
        /// 現在レーンをクリックした場合に消されるノーツを取得
        /// </summary>
        public NodeObject GetClickNode(int lane)
        {
            if (!StageTimeController.I.IsPlaying.CurrentValue) return null;

            NodeObject targetNode = null;
            float bestDifference = float.MaxValue;

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
                }
            }

            if (targetNode == null) return null;

            if (bestDifference <= _judgementTable.ToleranceValue)
            {
                return targetNode;
            }
            return null;
        }

        public void ClickNode(NodeObject targetNode)
        {
            targetNode.Release();
            _nodes.Remove(targetNode);
            _nodeHitExecutor.HandleHit(targetNode);
        }

        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.FirstOrDefault(x => x.NodeData.NodeID == nodeID);
        }
    }
}