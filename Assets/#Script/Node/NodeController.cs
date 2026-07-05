using System.Collections.Generic;
using System.Linq;
using Common.PlaySystem;
using InGame.UI;
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
        [SerializeField] private LaneClick _laneClick;

        private List<NodeObject> _nodes = new();

        private readonly Subject<(IReadOnlyJudgementData, int)> _showJudge = new();

        public Observable<(IReadOnlyJudgementData, int)> ShowJudge => _showJudge;
        private float _nextFillJudge;
        private float _fillJudgeIndex = 0;


        private SESoundType NormalSE;
        private SESoundType FlickSE;
        private SESoundType HoldStart;
        private SESoundType HoldFill;
        private SESoundType HoldEnd;
        public void Start()
        {
            CustomSoundPattern soundPattern = SongPlayManager.I.PatternData.SoundPattern;

            NormalSE = _soundData.TapSE[soundPattern.NormalSE].Value;
            FlickSE = _soundData.TapSE[soundPattern.FlickSE].Value;
            HoldStart = _soundData.TapSE[soundPattern.HoldStart].Value;
            HoldFill = _soundData.TapSE[soundPattern.HoldFill].Value;
            HoldEnd = _soundData.TapSE[soundPattern.HoldEnd].Value;

            InputManager.LeftLane.Where(_ => !InputManager.FlickLeftLane.CurrentValue).Subscribe(b => ClickLane(0, b, false)).AddTo(this);
            InputManager.RightLane.Where(_ => !InputManager.FlickRightLane.CurrentValue).Subscribe(b => ClickLane(1, b, false)).AddTo(this);
            InputManager.OnFlick.Subscribe(x => ClickLane(x, false, true)).AddTo(this);
        }

        public void AddNode(NodeObject node)
        {
            _nodes.Add(node);
        }

        public void Update()
        {
            if (!StageTimeController.IsPlaying) return;

            List<NodeObject> removeNode = new();
            foreach (NodeObject node in _nodes)
            {
                float deleteTime = StageTimeController.StageTime - JudgementManager.I.DeleteTime;

                if (node.NodeData.Time <= deleteTime)
                {
                    removeNode.Add(node);
                }
                float startTime = node.NodeData.Time - NodeGenerator.I.ArrivalSeconds;

                float progress = (StageTimeController.StageTime - startTime) / (node.NodeData.Time - startTime);

                Vector3 startPosition = node.StartPosition;
                Vector3 endPosition = startPosition;
                endPosition.z = node.GoalPos;

                node.transform.position = Vector3.LerpUnclamped(startPosition, endPosition, progress);
            }

            foreach (NodeObject node in removeNode)
            {
                if (node.Type != PoolPrefabType.Line)
                {
                    float difference = node.NodeData.Time - StageTimeController.StageTime;
                    var judgeData = ScoreManager.I.AddScore(node.Type, difference, node.NodeData);
                    _showJudge.OnNext((judgeData, node.NodeData.Lane));
                    if (node.Type == PoolPrefabType.HoldNoteEnd)
                    {
                        _nodeFillManager.DeleteFill(node.NodeData);
                    }
                }
                node.Release();
                _nodes.Remove(node);
            }
            if (_nextFillJudge <= StageTimeController.StageTime)
            {
                HoldLane(0, InputManager.LeftLane.CurrentValue);
                HoldLane(1, InputManager.RightLane.CurrentValue);

                _fillJudgeIndex++;
                _nextFillJudge = _fillJudgeIndex * 30f / StageTimeController.I.BPM;
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
        public void ClickLane(int lane, bool isClick, bool isFlick)
        {
            var node = GetClickNode(lane);

            bool isNodeClick = false;

            if (node != null)
            {
                if (isFlick)
                {
                    isNodeClick |= ClickNode(PoolPrefabType.FlickNote, FlickSE, node);
                }
                else if (isClick)
                {
                    isNodeClick |= ClickNode(PoolPrefabType.NormalNote, NormalSE, node);
                    isNodeClick |= ClickNode(PoolPrefabType.HoldNoteStart, HoldStart, node);
                }
                else
                {
                    isNodeClick |= ClickNode(PoolPrefabType.HoldNoteEnd, HoldEnd, node);
                }
            }

            if (!isNodeClick)
            {
                isNodeClick |= _nodeFillManager.HasFill(lane);
            }

            if (!isNodeClick && isClick && !isFlick && (node == null || node.NodeData.PrefabType != PoolPrefabType.FlickNote))
            {
                SoundManager.I.PlaySESound(SESoundType.EmptyHit);
                _laneClick.PlayLaneHighlight(lane);
            }
        }

        private NodeObject GetClickNode(int lane)
        {
            if (!StageTimeController.IsPlaying) return null;

            NodeObject targetNode = null;
            float bestDifference = float.MaxValue;
            float nodeTime = 0;

            foreach (var node in _nodes)
            {
                if (node.NodeData.Lane != lane) continue;

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

        private bool ClickNode(PoolPrefabType nodeType, SESoundType se, NodeObject targetNode)
        {
            if (targetNode.NodeData.PrefabType != nodeType) return false;

            float nodeTime = targetNode.NodeData.Time;

            SoundManager.I.PlaySESound(se);

            _nodes.Remove(targetNode);
            if (targetNode.NodeData.PrefabType == PoolPrefabType.HoldNoteEnd)
            {
                _nodeFillManager.DeleteFill(targetNode.NodeData);
            }
            targetNode.Release();

            var tapEffect = PoolManager.I.Get<PoolObject>(targetNode.TapEffect);
            Vector3 pos = targetNode.transform.position;
            pos.z = _goalPos;
            tapEffect.transform.position = pos;

            //var flash = PoolManager.I.Get<FlashEffect>(PoolPrefabType.FlashEffect);
            //flash.SetColor(targetNode.NodeColor);
            //flash.PlayFlash();
            //flash.transform.position = pos;

            float difference = nodeTime - StageTimeController.StageTime;
            var judgeData = ScoreManager.I.AddScore(targetNode.Type, difference, targetNode.NodeData);
            _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));

            _laneClick.PlayNodeClickEffect(targetNode.NodeData.Lane);

            return true;
        }

        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.Where(x => x.NodeData.NodeID == nodeID).FirstOrDefault();
        }
    }
}