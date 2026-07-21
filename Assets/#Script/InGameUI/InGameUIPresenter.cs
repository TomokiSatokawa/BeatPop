using InGame.Node;
using InGame.Score;
using R3;
using UnityEngine;

namespace InGame.UI
{
    /// <summary>
    /// InGameUIのPresenter
    /// </summary>
    public class InGameUIPresenter : MonoBehaviour
    {
        [SerializeField] private JudgeUIView _judgeUIControl;
        [SerializeField] private ComboUIControl _comboUIControl;
        [SerializeField] private NodeHitExecutor _nodeHitExecutor;
        [SerializeField] private ClearAnimation _clearAnimation;
        [SerializeField] private ScoreUIView _scoreUIView;
        [SerializeField] private RankUIControl _rankUIControl;
        [SerializeField] private MissAnimation _missAnimation;
        [SerializeField] private PauseUIControl _pauseUIControl;

        public void Start()
        {
            SubscribeJudge();
            SubscribeCombo();
            SubscribeScore();
            SubscribeClear();
            SubscribePause();
        }

        private void SubscribeJudge()
        {
            //判定
            _nodeHitExecutor.ShowJudge.Subscribe(data => _judgeUIControl.PlayAnimation(data.Judge.Name.ToString(), data.Judge.TextColor)).AddTo(this);
        }

        private void SubscribeCombo()
        {
            //コンボ
            ScoreDataManager.ScoreData.Combo.Where(x => x > 0).Subscribe(x => _comboUIControl.PlayComboAnimation(x)).AddTo(this);
            ScoreDataManager.ScoreData.Combo.Where(x => x <= 0).Subscribe(_ =>
            {
                _comboUIControl.Hide();
                _missAnimation.PlayAnimation();
            }).AddTo(this);
        }

        private void SubscribeScore()
        {
            //スコア表示
            ScoreDataManager.ScoreData.Score.Subscribe(x =>
            {
                float scoreRate = 0;
                if (x > 0)
                {
                    scoreRate = (float)x / ScoreDataManager.ScoreData.MaxScore;
                }

                _scoreUIView.UpdateScore(x, scoreRate);
                _rankUIControl.OnAnimation(scoreRate);

            }).AddTo(this);
        }

        private void SubscribeClear()
        {
            //クリア演出
            StageTimeController.I.OnGameClear.Subscribe(_ =>
            _clearAnimation.StartAnimation(() => GameManager.I.Clear())).AddTo(this);
        }

        private void SubscribePause()
        {
            //ポーズイベント
            _pauseUIControl.OnStartCountDown += GameManager.I.ReStartCountDown;
            _pauseUIControl.OnReStart += GameManager.I.ReStartStage;
            _pauseUIControl.OnRetry += GameManager.I.Retry;
            _pauseUIControl.OnReturnTitle += GameManager.I.ReturnTitle;
        }

        private void OnDestroy()
        {
            _pauseUIControl.OnStartCountDown -= GameManager.I.ReStartCountDown;
            _pauseUIControl.OnReStart -= GameManager.I.ReStartStage;
            _pauseUIControl.OnRetry -= GameManager.I.Retry;
            _pauseUIControl.OnReturnTitle -= GameManager.I.ReturnTitle;
        }
    }
}