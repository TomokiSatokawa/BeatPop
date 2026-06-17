using System.Collections.Generic;
using System.Linq;
using Input;
using R3;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    public class NodeController : MonoBehaviour
    {
        [SerializeField] private NodeJudgement _nodeJudgement;
        [SerializeField] private float _nodeSpeed;
        [SerializeField] private float _goalPos;
        [SerializeField] private HoldNodeFillManager _nodeFillManager;

        private List<NodeObject> _nodes = new();

        private readonly Subject<(IReadOnlyJudgementData, int)> _showJudge = new();

        public Observable<(IReadOnlyJudgementData, int)> ShowJudge => _showJudge;
        private float _nextFillJudge;
        private float _fillJudgeIndex = 0;

        public void Start()
        {
            InputManager.LeftLane.Where(b => b).Subscribe(_ => ClickLane(0, PoolPrefabType.NormalNote, SESoundType.NormalTap)).AddTo(this);
            InputManager.RightLane.Where(b => b).Subscribe(_ => ClickLane(1, PoolPrefabType.NormalNote, SESoundType.NormalTap)).AddTo(this);
            InputManager.LeftLane.Where(b => b).Subscribe(_ => ClickLane(0, PoolPrefabType.HoldNoteStart, SESoundType.HoldStart)).AddTo(this);
            InputManager.RightLane.Where(b => b).Subscribe(_ => ClickLane(1, PoolPrefabType.HoldNoteStart, SESoundType.HoldStart)).AddTo(this);
            InputManager.LeftLane.Where(b => !b).Subscribe(_ => ClickLane(0, PoolPrefabType.HoldNoteEnd, SESoundType.HoldEnd)).AddTo(this);
            InputManager.RightLane.Where(b => !b).Subscribe(_ => ClickLane(1, PoolPrefabType.HoldNoteEnd, SESoundType.HoldEnd)).AddTo(this);

            InputManager.FlickLeftLane.Where(b => b && InputManager.LeftLane.CurrentValue).Subscribe(_ => ClickLane(0, PoolPrefabType.FlickNote, SESoundType.FlickTap)).AddTo(this);
            InputManager.FlickRightLane.Where(b => b && InputManager.RightLane.CurrentValue).Subscribe(_ => ClickLane(1, PoolPrefabType.FlickNote, SESoundType.FlickTap)).AddTo(this);
        }

        public void AddNode(NodeObject node)
        {
            _nodes.Add(node);
        }

        public void Update()
        {
            List<NodeObject> removeNode = new();
            foreach (NodeObject node in _nodes)
            {
                float deleteTime = GameManager.I.StageTime - _nodeJudgement.DeleteTime;

                if (node.NodeData.Time <= deleteTime)
                {
                    removeNode.Add(node);
                }
                Vector3 position = node.transform.position;
                position.z -= node.MoveAmount * Time.deltaTime;
                node.transform.position = position;
            }

            foreach (NodeObject node in removeNode)
            {
                if (node.Type != PoolPrefabType.Line)
                {
                    float difference = node.NodeData.Time - GameManager.I.StageTime;
                    var judgeData = _nodeJudgement.JudgementDifference(difference);
                    _showJudge.OnNext((judgeData, node.NodeData.Lane));
                    ScoreManager.I.AddScore(judgeData,node.NodeData, difference);
                    if (node.Type == PoolPrefabType.HoldNoteEnd)
                    {
                        _nodeFillManager.DeleteFill(node.NodeData);
                    }
                }
                PoolManager.I.Release(node);
                _nodes.Remove(node);
            }
            if (_nextFillJudge <= GameManager.I.StageTime)
            {
                HoldLane(0,InputManager.LeftLane.CurrentValue);
                HoldLane(1, InputManager.RightLane.CurrentValue);

                _fillJudgeIndex++;
                _nextFillJudge = _fillJudgeIndex * 30f / GameManager.I.BPM;
            }
        }
        public void HoldLane(int lane,bool isHold)
        {
            if (_nodeFillManager.HasFill(lane))
            {
                var judgeData = _nodeJudgement.JudgementDifference(isHold ? 0:_nodeJudgement.ToleranceValue*2); 
                _showJudge.OnNext((judgeData, lane));
                    ScoreManager.I.AddHoldScore(judgeData);
            }
        }

        public void ClickLane(int lane, PoolPrefabType type, SESoundType se)
        {
            NodeObject targetNode = null;
            float bestDifference = float.MaxValue;
            float nodeTime = 0;

            foreach (var node in _nodes)
            {
                if (node.NodeData.Lane != lane) continue;
                if (node.Type != type) continue;

                float difference =
                    Mathf.Abs(node.NodeData.Time - GameManager.I.StageTime);

                if (difference < bestDifference)
                {
                    bestDifference = difference;
                    targetNode = node;
                    nodeTime = node.NodeData.Time;
                }
            }

            if (targetNode == null) return;

            if (bestDifference <= _nodeJudgement.ToleranceValue)
            {
                SoundManager.I.PlaySESound(se);

                _nodes.Remove(targetNode);
                if (targetNode.NodeData.PrefabType == PoolPrefabType.HoldNoteEnd)
                {
                    _nodeFillManager.DeleteFill(targetNode.NodeData);
                }
                PoolManager.I.Release(targetNode);
                float difference = nodeTime - GameManager.I.StageTime;
                var judgeData = _nodeJudgement.JudgementDifference(difference);
                _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));
                ScoreManager.I.AddScore(judgeData,targetNode.NodeData, difference);
            }
        }
        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.Where(x => x.NodeData.NodeID == nodeID).FirstOrDefault();
        }
    }
}