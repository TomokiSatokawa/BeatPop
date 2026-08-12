using System;
using System.Linq;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.PlayerData
{
    /// <summary>
    /// プレイ記録表のView
    /// </summary>
    public class PlayRecordView : MonoBehaviour
    {
        [SerializeField] private DifficultyColor _difficultyColor;
        [SerializeField] private Image[] _difficultyImages;
        [SerializeField] private DifficultyText _fullCombo;
        [SerializeField] private DifficultyText _allPerfect;

        private void Start()
        {
            for (int i = 0; i < _difficultyImages.Length; i++)
            {
                _difficultyImages[i].color = _difficultyColor.GetDifficultyColor((Difficulty)i);
            }
        }

        public void UpdateView()
        {
           var highScores = PlayerDataLoader.Records.HighScores;
            var difficultyDic = highScores.GroupBy(x => x.Difficulty).ToDictionary(x => (Difficulty)x.Key, x => x.ToArray());

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                int sumFullCombo = 0;
                int sumAllPerfect = 0;

                if (difficultyDic.TryGetValue(difficulty, out var records))
                {
                    sumFullCombo = records.Count(x => x.FullCombo);
                    sumAllPerfect = records.Count(x => x.AllPerfect);
                }

                _fullCombo.GetDifficultyText(difficulty).text = sumFullCombo.ToString();
                _allPerfect.GetDifficultyText(difficulty).text = sumAllPerfect.ToString();
            }
        }

        [Serializable]
        public struct DifficultyText
        {
            [SerializeField] private TextMeshProUGUI _easyText;
            [SerializeField] private TextMeshProUGUI _normalText;
            [SerializeField] private TextMeshProUGUI _hardText;
            [SerializeField] private TextMeshProUGUI _expertText;

            public TextMeshProUGUI GetDifficultyText(Difficulty difficulty)
            {
                var result = difficulty switch
                {
                    Difficulty.Easy => _easyText,
                    Difficulty.Normal => _normalText,
                    Difficulty.Hard => _hardText,
                    Difficulty.Expert => _expertText,
                    _ => null,
                };

                if (result == null)
                    Debug.LogError($"[PlayRecordView] Difficulty not found Difficulty:{difficulty}");

                return result;
            }
        }
    }
}