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

        public void SetNodeData(NodeData data)
        {
            NodeData = data;
        }

        public virtual void Update() { }
    }
}