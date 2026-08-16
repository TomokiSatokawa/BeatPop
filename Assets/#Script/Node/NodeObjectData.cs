using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ノーツごとの設定データ
    /// </summary>
    [System.Serializable]
    public class NodeObjectData : IReadOnlyNodeObjectData
    {
        [SerializeField] private PoolPrefabType _tapEffect;
        [SerializeField] private Color _nodeColor;
        [SerializeField] private float _emissionPower;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private InputType _inputType;

        public MeshRenderer MeshRenderer => _meshRenderer;
        public PoolPrefabType TapEffect => _tapEffect;
        public Color NodeColor => _nodeColor;
        public float EmissionPower => _emissionPower;

        public InputType InputType => _inputType;
    }

    public interface IReadOnlyNodeObjectData
    {
        public PoolPrefabType TapEffect { get; }
        public Color NodeColor { get; }
        public float EmissionPower { get; }
        public InputType InputType { get; }
    }
}