using InGame.Node;
using InGame;
using UnityEngine;

public class NodeObject : PoolObject
{
    [SerializeField] private NodeObjectData _objectData;
    public IReadOnlyNodeObjectData NodeObjData => _objectData;
    public NodeData NodeData;
    public Vector3 StartPosition;

    public float MoveAmount;

    public float GoalPos => _goalZ;
    private float _goalZ;
    /// <summary>
    /// arrivalSeconds•bŒã‚ÉtargetZ‚Ö“ž’B‚·‚é‘¬“x‚ðŒvŽZ
    /// </summary>
    public void SetMoveData(NodeData data, Vector3 startPosition)
    {
        NodeData = data;
        StartPosition = startPosition;
    }

    public virtual void Update() { }

    public void SetGoal(float goalZ)
    {
        _goalZ = goalZ; float distance = StartPosition.z - _goalZ;
        float remainTime = NodeData.Time - StageTimeController.StageTime;
        MoveAmount = distance / remainTime;
    }
}