using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RankData", menuName = "Scriptable Objects/RankData")]
public class RankData : ScriptableObject
{
    [SerializeField] private SerializableDictionary<Sprite, float> _rankValue;
    public Sprite GetRank(float rate)
    {
        foreach(var kv in _rankValue.Items)
        {
            if(kv.Value <= rate)
            {
                return kv.Key;
            }
        }
        Debug.LogError("Runk is not found");
        return null;
    }
}
