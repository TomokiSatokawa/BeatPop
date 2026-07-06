using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public bool IsPoolActive { get; private set; }
    private PoolPrefabType _poolPrefabType;
    public PoolPrefabType Type => _poolPrefabType;
    public void SetData(PoolPrefabType poolPrefabType)
    {
        _poolPrefabType = poolPrefabType;
    }
    public virtual void OnPoolActive()
    {
        IsPoolActive = true;
    }
    public virtual void OnPoolInactive()
    {
        IsPoolActive = false;
    }

    public void Release()
    {
        PoolManager.I.Release(this);
    }
}
