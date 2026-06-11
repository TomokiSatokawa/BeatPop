using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private PoolPrefabType _poolPrefabType;
    public PoolPrefabType Type => _poolPrefabType;
    public void SetData(PoolPrefabType poolPrefabType)
    {
        _poolPrefabType = poolPrefabType;
    }
}
