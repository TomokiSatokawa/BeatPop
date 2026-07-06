using InGame.Node;
using UnityEngine;

[System.Serializable]
public class NodeObjectData : IReadOnlyNodeObjectData
{
    [SerializeField] private PoolPrefabType _tapEffect;
    [SerializeField] private Color _nodeColor;
    [SerializeField] private InputType _inputType;

    public PoolPrefabType TapEffect => _tapEffect;

    public Color NodeColor => _nodeColor;

    public InputType InputType => _inputType;
}
public interface IReadOnlyNodeObjectData
{
    public PoolPrefabType TapEffect { get; }
    public Color NodeColor { get; }
    public InputType InputType { get; }
}