using UnityEngine;

[CreateAssetMenu(fileName = "JudgementTable", menuName = "Scriptable Objects/JudgementTable")]
public class JudgementTable : ScriptableObject
{
    [SerializeField] private float _deleteTime;
    public float DeleteTime => _deleteTime;
    [SerializeField] private float _toleranceValue;
    public float ToleranceValue => _toleranceValue;
    [SerializeField] private SerializableDictionary<PoolPrefabType, NodeJudgement> _nodeTypeJudge;

    public IReadOnlyJudgementData GetJudgement(PoolPrefabType type, float difference)
    {
        if (!_nodeTypeJudge.TryGetValue(type, out var judgementData))
        {
            Debug.LogError($"[ScoreDataManager] judge is not found  Type:{type}");
            return null;
        }
        return judgementData.JudgementDifference(difference);
    }
}
