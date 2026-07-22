using InGame.Score;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// ƒŠƒUƒ‹ƒg‚ÌPresenter
    /// </summary>
    public class ResultUIPresenter : MonoBehaviour
    {
        [SerializeField] private JudgementCountView _judgementCount;
        [SerializeField] private ScoreUIView _scoreUIControl;
        [SerializeField] private RankUIControl _rankUIControl;

        private void Start()
        {
            int score = ScoreDataManager.ScoreData.Score.CurrentValue;
            int maxScore = ScoreDataManager.ScoreData.MaxScore;
            float rate = maxScore > 0 ? score / (float)maxScore : 0f;

            _judgementCount.OnAnimation(ScoreDataManager.JudgementRecorder.JudgeDataCount);
            _scoreUIControl.OnAnimation(score, maxScore);
            _rankUIControl.OnAnimation(rate);
        }
    }
}