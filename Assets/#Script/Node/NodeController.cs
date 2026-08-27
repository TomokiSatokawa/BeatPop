using System.Collections.Generic;
using System.Linq;
using Input;
using R3;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// NodeGeneratorで生成したノーツを動かす
    /// </summary>
    public class NodeController : MonoBehaviour
    {
        private readonly List<NodeObject> _nodes = new();
        private readonly List<NodeObject> _removeNodes = new();
        private readonly List<NodeObject> _tickNodeHit = new();
        private readonly Subject<NodeObject> _onRemoveNode = new();
        private readonly Subject<NodeObject> _onHitNode = new();

        public Observable<NodeObject> OnRemoveNode => _onRemoveNode;
        public Observable<NodeObject> OnHitNode => _onHitNode;

        public void AddNode(NodeObject node)
        {
            _nodes.Add(node);
        }

        private void Update()
        {
            if (!StageTimeController.I?.IsPlaying.CurrentValue ?? true) return;

            UpdateNodes();
            ClickHitNode();
            RemoveExpiredNodes();
        }

        private void UpdateNodes()
        {
            _removeNodes.Clear();

            float stageTime = StageTimeController.StageTime;
            float deleteTime = stageTime - StageConfig.I.JudgementTable.DeleteTime;

            foreach (NodeObject node in _nodes)
            {
                Debug.Log(node);
                if (node.NodeData.Time <= deleteTime)
                {
                    _removeNodes.Add(node);
                }
                float startTime = node.NodeData.Time - StageConfig.I.ArrivalSeconds;

                float progress = (stageTime - startTime) / (node.NodeData.Time - startTime);

                Vector3 startPosition = StageConfig.I.GetClonePos(node.NodeData.Lane);
                Vector3 endPosition = startPosition;
                endPosition.z = StageConfig.I.StageLayout.GoalPos;
                node.transform.position = Vector3.LerpUnclamped(startPosition, endPosition, progress);

                if (node.Type == PoolPrefabType.TickNode && TickNodeCheck(node))
                {
                    _tickNodeHit.Add(node);
                }
            }
        }

        private void ClickHitNode()
        {
            foreach (NodeObject node in _tickNodeHit)
            {
                ClickNode(node);
            }
            _tickNodeHit.Clear();
        }

        private void RemoveExpiredNodes()
        {
            foreach (NodeObject node in _removeNodes)
            {
                if (node.Type != PoolPrefabType.Line)
                {
                    _onRemoveNode.OnNext(node);
                }
                node.Release();
                _nodes.Remove(node);
            }
        }

        private bool TickNodeCheck(NodeObject node)
        {
            if (node.NodeData.Time <= StageTimeController.StageTime)
            {
                if (node.NodeData.Lane == 0 && InputManager.LeftLane.CurrentValue)
                {
                    return true;
                }
                else if (node.NodeData.Lane == 1 && InputManager.RightLane.CurrentValue)
                {
                    return true;
                }
            }

            return false;
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

            if (bestDifference <= StageConfig.I.JudgementTable.ToleranceValue)
            {
                return targetNode;
            }
            return null;
        }

        public void ClickNode(NodeObject targetNode)
        {
            targetNode.Release();
            _nodes.Remove(targetNode);
            _onHitNode.OnNext(targetNode);
        }

        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.FirstOrDefault(x => x.NodeData.NodeID == nodeID);
        }
    }
}