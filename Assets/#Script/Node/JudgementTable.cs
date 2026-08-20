using Common.PlaySystem;
using Title.Custom;
using UnityEngine;

namespace InGame.Node
{
    /// <summary>
    /// ƒm[ƒc•Κ”»’θ‚π‚ά‚Ζ‚ί‚ι
    /// </summary>
    [CreateAssetMenu(fileName = "JudgementTable", menuName = "Scriptable Objects/JudgementTable")]
    public class JudgementTable : ScriptableObject
    {
        [SerializeField,Header("Α‚·Τ")] private float _deleteTime;
        [SerializeField, Header("”»’θ‚π‚·‚ιθ‡’l")] private float _toleranceValue;
        [SerializeField] private SerializableDictionary<PoolPrefabType, NodeJudgement> _nodeTypeJudge;

        public float DeleteTime => _deleteTime;
        public float ToleranceValue => _toleranceValue;

        public IReadOnlyJudgementData GetJudgementResult(PoolPrefabType type, float difference)
        {
            if (!_nodeTypeJudge.TryGetValue(type, out var judgementData))
            {
                Debug.LogError($"[JudgementTable] Judgement is not found. Type:{type}");
                return null;
            }
            var judgeLevel = SongPlayContext.I?.PatternData.JudgePattern.GetLevel(CustomJudge.PoolPrefabToCutomJudge(type)) ?? 0;
            return judgementData.GetJudgement(difference, judgeLevel);
        }

        public IReadOnlyNodeJudgement GetJudgementData(PoolPrefabType type)
        {
            if (!_nodeTypeJudge.TryGetValue(type, out var judgementData))
            {
                Debug.LogError($"[JudgementTable] Judgement is not found. Type:{type}");
                return null;
            }
            return judgementData;
        }
    }
}