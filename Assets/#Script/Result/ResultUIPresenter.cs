using Common.PlaySystem;
using Common.UI;
using InGame.Score;
using Title.PlayerData;
using Title.SongSelect;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// ƒŠƒUƒ‹ƒg‚ÌPresenter
    /// </summary>
    public class ResultUIPresenter : MonoBehaviour
    {
        [SerializeField] private PanelControl _playerPanel;
        [SerializeField] private JudgementCountView _judgementCount;
        [SerializeField] private ScoreUIView _scoreUIControl;
        [SerializeField] private RankUIControl _rankUIControl;
        [SerializeField] private ResultSongInfo _songInfo;
        [SerializeField] private ResultBadgeView _badgeView;
        [SerializeField] private NodeAccuracyView _accuracyView;
        [SerializeField] private TimingSliderView _timingSliderView;
        [SerializeField] private PanelSlider _panelSlider;
        [SerializeField] private LevelAnimation _levelAnimation;
        [SerializeField] private XpSliderAnimation _xpSliderAnimation;

        private void Start()
        {
            int score = ScoreDataManager.ScoreData.Score.CurrentValue;
            int maxScore = ScoreDataManager.ScoreData.MaxScore;
            var songData = SongPlayContext.I.SongData;
            var resultDataCollector = ScoreDataManager.ResultDataCollector;
            float rate = maxScore > 0 ? score / (float)maxScore : 0f;

            _judgementCount.OnAnimation(ScoreDataManager.JudgementRecorder.JudgeDataCount);
            _scoreUIControl.OnAnimation(score, maxScore);
            _rankUIControl.OnAnimation(rate, true);
            _songInfo.ShowInfo(songData);
            _badgeView.ShowBadge(ScoreDataManager.ScoreData.GetResultType());
            _accuracyView.OnAnimation(resultDataCollector.NodeHitCount);
            _timingSliderView.OnAnimation(resultDataCollector.FastCount, resultDataCollector.LateCount);

            PlayerDataLoader.Records.SavePlayData(songData, score);
            PlayerDataLoader.Records.SaveHighScore(songData, score,out var highScore);
        }

        public void ShowPlayerPanel()
        {
            _panelSlider.ChangePanel(_playerPanel);

            _xpSliderAnimation.Play(ResultManager.PreviousPlayerInfo, PlayerDataLoader.Info, _levelAnimation.Play);
        }

    }
}
