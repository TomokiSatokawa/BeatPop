using InGame.Node;
using UnityEngine;

public class NodeObject : PoolObject
{
    public NodeData NodeData;
    public Vector3 StartPosition;

    public float MoveAmount;

    private float _goalZ;
    /// <summary>
    /// arrivalSeconds•bŒã‚ÉtargetZ‚Ö“ž’B‚·‚é‘¬“x‚ðŒvŽZ
    /// </summary>
    public void SetMoveData(NodeData data)
    {
        NodeData = data;
        StartPosition = this.transform.position;
    }

    public virtual  void Update()
    {
        //if(NodeData.Time <= GameManager.I.StageTime + NodeGenerator.I.ArraivalSeconds)
        //{
        //    float distance = StartPosition.z - _goalZ;
        //    float remainTime = NodeData.Time - GameManager.I.StageTime;
        //    MoveAmount = distance / remainTime;
        //}
        //else
        //{
        //    MoveAmount = 0;
        //}
    }
    public void SetGoal(float goalZ)
    {
        _goalZ = goalZ; float distance = StartPosition.z - _goalZ;
        float remainTime = NodeData.Time - GameManager.I.StageTime;
        MoveAmount = distance / remainTime;
    }
}