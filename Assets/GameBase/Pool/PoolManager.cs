using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    [SerializeField] private PoolPrefabData _poolPrefabData;

    private readonly Dictionary<PoolPrefabType, Queue<PoolObject>> _pool = new();

    private PoolObject CreatePoolObject(PoolPrefabType type,Transform parent)
    {
        PoolObject prefab = _poolPrefabData.GetPrefab(type);

        PoolObject instance = Instantiate(prefab, transform,parent);
        instance.SetData(type);

        return instance;
    }

    public T Get<T>(PoolPrefabType type,Transform parent = null) where T : PoolObject
    {
        if (!_pool.TryGetValue(type, out Queue<PoolObject> queue))
        {
            queue = new Queue<PoolObject>();
            _pool.Add(type, queue);
        }

        PoolObject poolObject;

        if (queue.Count > 0)
        {
            poolObject = queue.Dequeue();
        }
        else
        {
            poolObject = CreatePoolObject(type,parent);
        }
        if (poolObject is not T result)
        {
            Debug.LogError($"PoolObject type mismatch. " + $"PoolType: {type}, " + $"Expected: {typeof(T).Name}, " + $"Actual: {poolObject.GetType().Name}");
            return null;
        }

        result.transform.parent = parent;
        result.gameObject.SetActive(true);

        return result;
    }

    public void Release(PoolObject poolObject)
    {
        PoolPrefabType type = poolObject.Type;

        if (!_pool.TryGetValue(type, out Queue<PoolObject> queue))
        {
            queue = new Queue<PoolObject>();
            _pool.Add(type, queue);
        }

        poolObject.gameObject.SetActive(false);
        queue.Enqueue(poolObject);
    }
}