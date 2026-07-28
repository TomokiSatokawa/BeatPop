using Common;
using Title.SongSelect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Result.UI
{
    /// <summary>
    /// Result‚Ì‹ÈŠT—v
    /// </summary>
    public class ResultSongInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _songName;
        [SerializeField] private TextMeshProUGUI _difficultName;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _difficultNameBackGround;
        [SerializeField] private Image _jacket;
        [SerializeField] private DifficultyColor _difficultyColor;
        [SerializeField] private Image _difficultyImage;
        public void ShowInfo(SongSelectData songData)
        {
            _songName.text = songData.SongData.SongName;
            _difficultName.text = songData.Difficulty.ToString();
            _levelText.text = $"Lv.{songData.SongData.Charts.GetLevel(songData.Difficulty)}";
            _jacket.sprite = songData.SongData.Jacket;

            Color difficultyColor = _difficultyColor.GetDifficultyColor(songData.Difficulty);
            _difficultyImage.color = difficultyColor;
            _difficultNameBackGround.color = difficultyColor;
        }
    }
}