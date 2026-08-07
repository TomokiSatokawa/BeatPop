using Result.UI;
using Title.SongSelect;
using UnityEngine;

namespace Result
{
    /// <summary>
    /// 報酬データ
    /// </summary>
    [CreateAssetMenu(fileName = "ResultRewardData", menuName = "Scriptable Objects/ResultRewardData")]
    public class ResultRewardData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<Difficulty, int> _difficultyCoin;
        [SerializeField] private SerializableDictionary<RankType, float> _rankCoinMultiplier;
        [SerializeField] private SerializableDictionary<ResultType, float> _comboMultiplier;

        public int GetCoinCount(Difficulty difficulty, RankType rank, ResultType resultType)
        {
            if (!_difficultyCoin.TryGetValue(difficulty, out var baseCoin))
            {
                Debug.LogError($"[ResultRewardData] Difficultyのコイン設定がありません。Difficulty: {difficulty}");
                return 0;
            }

            if (!_rankCoinMultiplier.TryGetValue(rank, out var rankMultiplier))
            {
                Debug.LogError($"[ResultRewardData] Rankのコイン倍率設定がありません。Rank: {rank}");
                return 0;
            }

            if (!_comboMultiplier.TryGetValue(resultType, out var comboMultiplier))
            {
                Debug.LogError($"[ResultRewardData] ResultTypeのコンボ倍率設定がありません。ResultType: {resultType}");
                return 0;
            }

            return Mathf.FloorToInt(baseCoin * rankMultiplier * comboMultiplier);
        }
    }
}