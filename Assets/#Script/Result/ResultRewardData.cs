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
        [SerializeField] private int[] _rankUpJewelryCount;
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

        public int GetJewelryCount(int start, int end)
        {
            int result = 0;
            for(int i = start -1 ; i < end; i++)
            {
                int index = i;
                if (i < 0 || i >= _rankUpJewelryCount.Length)
                {
                    Debug.LogError($"[ResultRewardData] 入力されていないレベルを取得しようとしました。  level:{i + 1}");
                    continue;
                }

                result += _rankUpJewelryCount[index];
            }

            return result;  
        }
    }
}