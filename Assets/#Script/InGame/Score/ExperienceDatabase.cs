using Title.SongSelect;
using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceDatabase", menuName = "Scriptable Objects/ExperienceDatabase")]
public class ExperienceDatabase : ScriptableObject
{
    [SerializeField] private int _comboBonus;
    public int ComboBonus => _comboBonus;
    [SerializeField] private SerializableDictionary<RankType, int> _rankBonus;

    public int GetRankBonus(RankType rank)
    {
        //ランクボーナスを検索
        if (!_rankBonus.TryGetValue(rank, out int value))
        {
            Debug.LogError($"[ExperienceDatabase] rank not found rank:{rank}");
            return 0;
        }
        return value;
    }
}
