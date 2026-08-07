using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int[] _levelUpXp;

    public int GetLevelUpXp(int level)
    {
        if(level < 0)
        {
            Debug.LogError("[LevelData] 0–¢–ž‚ÌƒŒƒxƒ‹");
            return 0;
        }

        var index = Mathf.Clamp(level - 1, 0,_levelUpXp.Length -1);
        return _levelUpXp[index];
    }

    public void LevelUp(int startLevel, int startXp, int addXp, out int endLevel, out int endXp)
    {
        Debug.Log(addXp);

        endLevel = startLevel;
        endXp = startXp + addXp;

        while (endXp >= GetLevelUpXp(endLevel))
        {
            endXp -= GetLevelUpXp(endLevel);
            endLevel++;

            // Å‘åƒŒƒxƒ‹
            if (endLevel >= _levelUpXp.Length)
            {
                endLevel = _levelUpXp.Length - 1;
                endXp = 0;
                break;
            }
        }
    }
}
