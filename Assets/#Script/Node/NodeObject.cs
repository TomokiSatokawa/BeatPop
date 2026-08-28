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
        public float StartTime { get; private set; }

        public void Start()
        {
            SetColor(InGameCustomColorData.I.GetNodeColor(Type));
        }

        public void SetColor(Color color)
        {
            foreach (var material in _objectData.MeshRenderer.materials)
            {
                // ベースカラー
                material.SetColor("_BaseColor", color);

                // エミッションカラー
                material.SetColor("_EmissionColor", color * _objectData.EmissionPower);
            }
        }

        public void SetNodeData(NodeData data, float startTime)
        {
            NodeData = data;
            StartTime = startTime;
        }

        public virtual void Update() { }
    }
}