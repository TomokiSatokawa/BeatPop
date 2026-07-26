using Common.PlaySystem;
using InGame.Score;
using Title.SongSelect;
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
        [SerializeField] private ResultSongInfo _songInfo;
        [SerializeField] private ResultBadgeView _badgeView;

        private void Start()
        {
            int score = ScoreDataManager.ScoreData.Score.CurrentValue;
            int maxScore = ScoreDataManager.ScoreData.MaxScore;
            float rate = maxScore > 0 ? score / (float)maxScore : 0f;

            _judgementCount.OnAnimation(ScoreDataManager.JudgementRecorder.JudgeDataCount);
            _scoreUIControl.OnAnimation(score, maxScore);
            _rankUIControl.OnAnimation(rate,true);
            _songInfo.ShowInfo(SongPlayContext.I.SongData);
            _badgeView.ShowBadge(ScoreDataManager.ScoreData.GetResultType());
        }
    }
}