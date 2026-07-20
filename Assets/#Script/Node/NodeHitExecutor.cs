using Common.BeatUpdate;
using InGame;
using InGame.Effect;
using InGame.Node;
using InGame.Score;
using Input;
using R3;
using Sound;
using UnityEngine;

/// <summary>
/// ノーツのHit処理
/// </summary>
public class NodeHitExecutor : MonoBehaviour
{
    [SerializeField] private HoldNodeFillManager _nodeFillManager;
    [SerializeField] private LaneClickEffect _laneClick;
    [SerializeField] private float _goalPos;

    private readonly Subject<(IReadOnlyJudgementData, int)> _showJudge = new();
    public Observable<(IReadOnlyJudgementData Judge, int lane)> ShowJudge => _showJudge;

    public void Start()
    {
        BeatUpdateManager.BeatUpdate.Subscribe(8, 0, _ =>
        {
            HoldLane(0, InputManager.LeftLane.CurrentValue);
            HoldLane(1, InputManager.RightLane.CurrentValue);
        });
    }
    public void HitAction(NodeObject targetNode)
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
        var judgeData = JudgeNode(targetNode);
        _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));

        _laneClick.PlayNodeClickEffect(targetNode.NodeData.Lane);

        var se = InGameCustomSoundData.I.NodeSE[targetNode.Type];
        SoundManager.SE.PlaySE(se, judgeData.TapSEVolume);
    }

    private static IReadOnlyJudgementData JudgeNode(NodeObject targetNode)
    {
        float difference = targetNode.NodeData.Time - StageTimeController.StageTime;
        var judgeData = ScoreDataManager.I.RecordJudge(targetNode.NodeData, difference);
        return judgeData;
    }

    public void RemoveAction(NodeObject targetNode)
    {
        var judgeData = JudgeNode(targetNode);

        _showJudge.OnNext((judgeData, targetNode.NodeData.Lane));
        if (targetNode.Type == PoolPrefabType.HoldNoteEnd)
        {
            _nodeFillManager.DeleteFill(targetNode.NodeData);
        }
    }
    public void HoldLane(int lane, bool isHold)
    {
        if (_nodeFillManager.HasFill(lane))
        {
            var nodeData = new NodeData()
            {
                Lane = lane,
                Time = StageTimeController.StageTime,
                PrefabType = PoolPrefabType.HoldNoteFill,
            };

            float difference = isHold ? 0 : float.MaxValue;
            var judgeData = ScoreDataManager.I.RecordJudge(nodeData, difference);
            _showJudge.OnNext((judgeData, lane));
        }
    }
}
