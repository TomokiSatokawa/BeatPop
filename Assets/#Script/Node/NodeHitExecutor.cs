using Common.BeatUpdate;
using InGame.Effect;
using InGame.Score;
using Input;
using R3;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ノーツのHit処理
    /// </summary>
    public class NodeHitExecutor : MonoBehaviour
    {
        [SerializeField] private HoldNodeFillManager _nodeFillManager;
        [SerializeField] private LaneClickEffect _laneClick;
        [SerializeField] private float _goalPos;//TODO:StageのSO

        private readonly Subject<(IReadOnlyJudgementData, int)> _showJudge = new();
        public Observable<(IReadOnlyJudgementData Judge, int lane)> ShowJudge => _showJudge;

        public void Start()
        {
            BeatUpdateManager.BeatUpdate.Subscribe(8, 0, _ =>
            {
                ExecuteHoldJudge(0, InputManager.LeftLane.CurrentValue);
                ExecuteHoldJudge(1, InputManager.RightLane.CurrentValue);
            });
        }

        public void HandleHit(NodeObject targetNode)
        {
            if (targetNode.NodeData.PrefabType == PoolPrefabType.HoldNoteEnd)
            {
                _nodeFillManager.DeleteFill(targetNode.NodeData);
            }

            //タップエフェクト
            var tapEffect = PoolManager.I.Get<PoolObject>(targetNode.NodeObjData.TapEffect);
            Vector3 pos = targetNode.transform.position;
            pos.z = _goalPos;
            tapEffect.transform.position = pos;

            //ジャッチUI
            var judgeData = ExecuteJudgeNode(targetNode.NodeData);

            _laneClick.PlayNodeClickEffect(targetNode.NodeData.Lane);

            //SE
            var se = InGameCustomSoundData.I.NodeSE[targetNode.Type];
            SoundManager.SE.PlaySE(se, judgeData.TapSEVolume);
        }

        public void HandleRemove(NodeObject targetNode)
        {
            ExecuteJudgeNode(targetNode.NodeData);

            if (targetNode.Type == PoolPrefabType.HoldNoteEnd)
            {
                _nodeFillManager.DeleteFill(targetNode.NodeData);
            }
        }

        public void ExecuteHoldJudge(int lane, bool isHold)
        {
            if (!_nodeFillManager.HasFill(lane)) return;

            var nodeData = new NodeData()
            {
                Lane = lane,
                Time = StageTimeController.StageTime,
                PrefabType = PoolPrefabType.HoldNoteFill,
            };

            float difference = isHold ? 0 : float.MaxValue;
            ExecuteJudgeNode(nodeData, difference);
        }

        private IReadOnlyJudgementData ExecuteJudgeNode(NodeData nodeData)
        {
            float difference = nodeData.Time - StageTimeController.StageTime;
            return ExecuteJudgeNode(nodeData, difference);
        }

        private IReadOnlyJudgementData ExecuteJudgeNode(NodeData nodeData, float difference)
        {
            var judgeData = ScoreDataManager.I.RecordJudge(nodeData, difference);
            _showJudge.OnNext((judgeData, nodeData.Lane));
            return judgeData;
        }
    }
}