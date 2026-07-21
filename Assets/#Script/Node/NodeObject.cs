using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ノーツオブジェクトのデータ保持
    /// </summary>
    public class NodeObject : PoolObject
    {
        [SerializeField] private NodeObjectData _objectData;
        public IReadOnlyNodeObjectData NodeObjData => _objectData;
        public NodeData NodeData { get; private set; }
        public Vector3 StartPosition { get; private set; }

        public void SetMoveData(NodeData data, Vector3 startPosition)
        {
            NodeData = data;
            StartPosition = startPosition;
        }

        public virtual void Update() { }
    }
}