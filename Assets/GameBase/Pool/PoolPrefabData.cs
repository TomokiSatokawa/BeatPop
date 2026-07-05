using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolPrefabData", menuName = "Scriptable Objects/PoolPrefabData")]
public class PoolPrefabData : ScriptableObject
{
    [SerializeField] private PrefabData[] _prefabDatas;
    [SerializeField] private int _maxTaskSpawnCount = 50;
    public IReadOnlyList<PrefabData> PrefabDatas => _prefabDatas;
    public int MaxTaskSpawnCount => _maxTaskSpawnCount;

    public Dictionary<PoolPrefabType, PoolObject> _prefabDic;
    [System.Serializable]
    public class PrefabData
    {
        public PoolPrefabType Type;
        public PoolObject Prefab;
        public int StartCloneCount = 5;
    }

    private void SetPrefabDic()
    {
        _prefabDic = new();

        foreach(var data in _prefabDatas)
        {
            _prefabDic.Add(data.Type, data.Prefab);
        }
    }
    public PoolObject GetPrefab(PoolPrefabType type)
    {
        if(_prefabDic == null) SetPrefabDic();

        if (!_prefabDic.TryGetValue(type, out var prefab)) 
            Debug.LogError($"{type.ToString()} ‚ÌPrefab‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");

        return prefab;
    }
}
