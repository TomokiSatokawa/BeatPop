using InGame.Node;
using UnityEngine;

public class HoldNode : NodeObject
{
    [SerializeField] private Transform _fillObject;
    public NodeData? EndData { get; private set; } = null;
    public void SetEndNode(NodeData endData)
    {
        EndData = endData;
    }
    public override void Update()
    {
        base.Update();

    }
}
