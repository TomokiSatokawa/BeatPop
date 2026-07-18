using UnityEngine;

public class JudgementManager : SingletonPersistent<JudgementManager>
{
    [SerializeField] private float _deleteTime;
    [SerializeField] private float _toleranceValue;
    [SerializeField] private SerializableDictionary<PoolPrefabType, NodeJudgement> _nodeTypeJudge;
    public float DeleteTime => _deleteTime;
    public float ToleranceValue => _toleranceValue;
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    public IReadOnlyJudgementData GetJudgement(PoolPrefabType type,float difference)
    {
        if(!_nodeTypeJudge.TryGetValue(type, out var judgementData))
        {
            Debug.LogError($"{type} judge is not found");
            return null;
        }
        return judgementData.JudgementDifference(difference);
    }
}
