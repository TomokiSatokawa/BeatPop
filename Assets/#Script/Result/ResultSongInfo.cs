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
        [SerializeField] private Image _jacket;
        [SerializeField] private DifficultyColor _difficultyColor;
        [SerializeField] private Image _difficultyImage;
        public void ShowInfo(SongSelectData songData)
        {
            _songName.text = songData.SongData.SongName;
            _jacket.sprite = songData.SongData.Jacket;
            _difficultyImage.color = _difficultyColor.GetDifficultyColor(songData.Difficulty);
        }
    }
}