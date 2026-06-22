using System.Collections.Generic;
using System.Linq;
using Common.PlaySystem;
using Input;
using R3;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    public class NodeController : MonoBehaviour
    {
        [SerializeField] private CustomSoundData _soundData;
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
            Debug.Log("Start");
            CustomSoundPattern soundPattern = SongPlayManager.I.PatternData.SoundPattern;

            var normalSE = _soundData.TapSE[soundPattern.NormalSE].Value;
            var flickSE = _soundData.TapSE[soundPattern.FlickSE].Value;
            var holdStart = _soundData.TapSE[soundPattern.HoldStart].Value;
            var holdFill = _soundData.TapSE[soundPattern.HoldFill].Value;
            var holdEnd = _soundData.TapSE[soundPattern.HoldEnd].Value;

            InputManager.LeftLane.Where(b => b).Subscribe(_ =>
            {
                ClickLane(0, PoolPrefabType.NormalNote, normalSE);
                ClickLane(0, PoolPrefabType.HoldNoteStart, holdStart);
                ClickLane(0, PoolPrefabType.HoldNoteEnd, holdEnd);
                if (InputManager.FlickLeftLane.CurrentValue)
                {
                    ClickLane(0, PoolPrefabType.FlickNote, flickSE);
                }
            }).AddTo(this);

            InputManager.RightLane.Where(b => b).Subscribe(_ =>
            {
                ClickLane(1, PoolPrefabType.NormalNote, normalSE);
                ClickLane(1, PoolPrefabType.HoldNoteStart, holdStart);
                ClickLane(1, PoolPrefabType.HoldNoteEnd, holdEnd);
                if (InputManager.FlickLeftLane.CurrentValue)
                {
                    ClickLane(1, PoolPrefabType.FlickNote, flickSE);
                }
            
            }).AddTo(this);

            InputManager.FlickLeftLane.Where(b => b && InputManager.LeftLane.CurrentValue).Subscribe(_ =>
            {
                ClickLane(0, PoolPrefabType.FlickNote, flickSE);
            }).AddTo(this);

            InputManager.FlickRightLane.Where(b => b && InputManager.RightLane.CurrentValue).Subscribe(_ => 
            {
                ClickLane(1, PoolPrefabType.FlickNote, flickSE);
            }).AddTo(this);
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
                float deleteTime = GameManager.I.StageTime - JudgementManager.I.DeleteTime;

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
                    var judgeData = ScoreManager.I.AddScore(node.Type, difference, node.NodeData);
                    _showJudge.OnNext((judgeData, node.NodeData.Lane));
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
                HoldLane(0, InputManager.LeftLane.CurrentValue);
                HoldLane(1, InputManager.RightLane.CurrentValue);

                _fillJudgeIndex++;
                _nextFillJudge = _fillJudgeIndex * 30f / GameManager.I.BPM;
            }
        }
        public void HoldLane(int lane, bool isHold)
        {
            if (_nodeFillManager.HasFill(lane))
            {
                float difference = isHold ? 0 : JudgementManager.I.ToleranceValue * 2;
                var judgeData = ScoreManager.I.AddHoldScore(PoolPrefabType.HoldNoteFill, difference);
                _showJudge.OnNext((judgeData, lane));
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

            if (bestDifference <= JudgementManager.I.ToleranceValue)
            {
                SoundManager.I.PlaySESound(se);

                _nodes.Remove(targetNode);
                if (targetNode.NodeData.PrefabType == PoolPrefabType.HoldNoteEnd)
                {
                    _nodeFillManager.DeleteFill(targetNode.NodeData);
                }
                PoolManager.I.Release(targetNode);
                var effect = PoolManager.I.Get<PoolObject>(PoolPrefabType.TapEffect);
                Vector3 pos = targetNode.transform.position;
                pos.z = _goalPos;
                effect.transform.position = pos;

                float difference = nodeTime - GameManager.I.StageTime;
                var judgeData = ScoreManager.I.AddScore(targetNode.Type, difference, targetNode.NodeData);
                _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));
            }
        }
        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.Where(x => x.NodeData.NodeID == nodeID).FirstOrDefault();
        }
    }
}