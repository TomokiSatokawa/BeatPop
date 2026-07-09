using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private CustomSoundData _soundData;
        [SerializeField] private float _nodeSpeed;
        [SerializeField] private float _goalPos;
        [SerializeField] private HoldNodeFillManager _nodeFillManager;
        [SerializeField] private LaneClick _laneClick;

        private List<NodeObject> _nodes = new();

        private readonly Subject<(IReadOnlyJudgementData, int)> _showJudge = new();

        public Observable<(IReadOnlyJudgementData, int)> ShowJudge => _showJudge;

        private readonly List<NodeObject> _removeNodes = new();
        private void Start()
        {
           StageTimeController.I.OnInitialized.Subscribe(_ => InitializeBeat()).AddTo(this);
        }

        private void InitializeBeat()
        {
            BeatUpdateManager.I.Register(new BeatUpdateHandle(8, 0, (_, _) =>
            {
                HoldLane(0, InputManager.LeftLane.CurrentValue);
                HoldLane(1, InputManager.RightLane.CurrentValue);
            }));
        }

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
        }
        private void HoldLane(int lane, bool isHold)
        {
            if (_nodeFillManager.HasFill(lane))
            {
                float difference = isHold ? 0 : JudgementManager.I.ToleranceValue * 2;
                var judgeData = ScoreManager.I.AddHoldScore(PoolPrefabType.HoldNoteFill, difference);
                _showJudge.OnNext((judgeData, lane));
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

        public void ClickNode(SESoundType se, NodeObject targetNode)
        {
            float nodeTime = targetNode.NodeData.Time;

            SoundManager.I.PlaySESound(se);

            _nodes.Remove(targetNode);

            if (targetNode.NodeData.PrefabType == PoolPrefabType.HoldNoteEnd)
            {
                _nodeFillManager.DeleteFill(targetNode.NodeData);
            }
            targetNode.Release();

            var tapEffect = PoolManager.I.Get<PoolObject>(targetNode.NodeObjData.TapEffect);
            Vector3 pos = targetNode.transform.position;
            pos.z = _goalPos;
            tapEffect.transform.position = pos;

            float difference = nodeTime - StageTimeController.StageTime;
            var judgeData = ScoreManager.I.AddScore(targetNode.Type, difference, targetNode.NodeData);
            _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));

            _laneClick.PlayNodeClickEffect(targetNode.NodeData.Lane);
        }

        public NodeObject GetClonedNode(int nodeID)
        {
            return _nodes.Where(x => x.NodeData.NodeID == nodeID).FirstOrDefault();
        }
    }
}