using UnityEngine;

public class JudgementManager : SingletonMonoBehaviour<JudgementManager>
{
    [SerializeField] private float _deleteTime;
    [SerializeField] private float _toleranceValue;
    [SerializeField] private SerializableDictionary<PoolPrefabType, NodeJudgement> _nodeTypeJudge;
    public float DeleteTime => _deleteTime;
    public float ToleranceValue => _toleranceValue;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public IReadOnlyJudgementData JudgementDifference(PoolPrefabType type,float difference)
    {
        if(!_nodeTypeJudge.TryGetValue(type, out var judgementData))
        {
            Debug.LogError($"{type} judge is not found");
            return null;
        }
        return judgementData.JudgementDifference(difference);
    }
    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
