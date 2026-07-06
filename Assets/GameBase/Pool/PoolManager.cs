using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    [SerializeField] private PoolPrefabData _poolPrefabData;

    private readonly Dictionary<PoolPrefabType, Queue<PoolObject>> _pool = new();

    public override void Awake()
    {
        base.Awake();
        foreach(var poolData in _poolPrefabData.PrefabDatas)
        {
            _pool.Add(poolData.Type, new());
        }
    }
    public async UniTask ClonePoolObject()
    {
        List<UniTask> tasks = new();

        int maxTaskSpawnCount = _poolPrefabData.MaxTaskSpawnCount;

        foreach (var poolData in _poolPrefabData.PrefabDatas)
        {
            int remain = poolData.StartCloneCount;

            while (remain > 0)
            {
                int spawnCount = Mathf.Min(remain, maxTaskSpawnCount);

                int count = spawnCount;
                var data = poolData;

                tasks.Add(CloneAndRelease(data, count));

                remain -= spawnCount;
            }
        }

        await UniTask.WhenAll(tasks);
    }

    private async UniTask CloneAndRelease(PoolPrefabData.PrefabData poolData, int count)
    {
        var poolObjects = await InstantiateAsync(poolData.Prefab, count);

        foreach (var poolObject in poolObjects)
        {
            poolObject.SetData(poolData.Type);
            Release(poolObject);
        }
    }
    private PoolObject CreatePoolObject(PoolPrefabType type, Transform parent)
    {
        PoolObject prefab = _poolPrefabData.GetPrefab(type);

        PoolObject instance = Instantiate(prefab, transform, parent);
        instance.SetData(type);

        return instance;
    }

    public T Get<T>(PoolPrefabType type, Transform parent = null) where T : PoolObject
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
            poolObject = CreatePoolObject(type, parent);
        }
        if (poolObject is not T result)
        {
            Debug.LogError($"PoolObject type mismatch. " + $"PoolType: {type}, " + $"Expected: {typeof(T).Name}, " + $"Actual: {poolObject.GetType().Name}");
            return null;
        }

        result.transform.SetParent(parent);
        result.gameObject.SetActive(true);
        result.OnPoolActive();

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
        poolObject.OnPoolInactive();
    }
}