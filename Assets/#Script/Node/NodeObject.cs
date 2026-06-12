using InGame.Node;
using UnityEngine;

public class NodeObject : PoolObject
{
    public NodeData NodeData;
    public Vector3 StartPosition;

    public float MoveAmount;
    /// <summary>
    /// arrivalSeconds•bŒã‚ÉtargetZ‚Ö“ž’B‚·‚é‘¬“x‚ðŒvŽZ
    /// </summary>
    public void SetMoveData(NodeData data)
    {
        NodeData = data;
        StartPosition = this.transform.position;
    }

    public void SetGoal(float goalZ)
    {
        float distance = StartPosition.z - goalZ;
        float remainTime = NodeData.Time - GameManager.I.StageTime;
        MoveAmount = distance / remainTime;

    }
}