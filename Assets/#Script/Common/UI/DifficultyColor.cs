using UnityEngine;

namespace Common
{
    /// <summary>
    /// 難易度別カラー
    /// </summary>
    [CreateAssetMenu(fileName = "DifficultyColor", menuName = "Scriptable Objects/DifficultyColor")]
    public class DifficultyColor : ScriptableObject
    {
        [SerializeField] private Color _easy;
        [SerializeField] private Color _normal;
        [SerializeField] private Color _hard;
        [SerializeField] private Color _expert;

        public Color Easy => _easy;
        public Color Normal => _normal;
        public Color Expert => _expert;
        public Color Hard => _hard;

        public Color GetDifficultyColor(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return _easy;
                case Difficulty.Normal:
                    return _normal;
                case Difficulty.Hard:
                    return _hard;
                case Difficulty.Expert:
                    return _expert;
                default:
                    Debug.LogError($"[DifficultyColor] 難易度に対応する色がありません。 Difficulty:{difficulty}");
                    return Color.white;
            }
        }
    }
}
