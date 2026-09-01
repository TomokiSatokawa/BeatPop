using TMPro;
using UnityEngine;

namespace Result
{
    /// <summary>
    /// ハイスコアUI
    /// </summary>
    public class HighScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI  _highScoreText;

        public void ShowHighScore()
        {
            _highScoreText.gameObject.SetActive(true);
        }
        public void HiddenHighScore()
        {
            _highScoreText.gameObject.SetActive(false);
        }
    }
}