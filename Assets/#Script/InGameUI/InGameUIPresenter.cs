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

        public void Start()
        {
            //判定
            _nodeHitExecutor.ShowJudge.Subscribe(data => _judgeUIControl.OnPlay(data.Judge.Name.ToString(), data.Judge.TextColor)).AddTo(this);

            //コンボ
            ScoreDataManager.ScoreData.Combo.Where(x => x > 0).Subscribe(x => _comboUIControl.UpdateCombo(x)).AddTo(this);
            ScoreDataManager.ScoreData.Combo.Where(x => x <= 0).Subscribe(_ =>
            {
                _comboUIControl.HiddenUI();
                _missAnimation.PlayAnimation();
            }).AddTo(this);

            //スコア表示
            _scoreUIView.SetData();
            ScoreDataManager.ScoreData.Score.Subscribe(x =>
            {
                _scoreUIView.UpdateScore(x);

                float scoreRate = 0;
                if (x > 0)
                {
                    scoreRate =(float)x / ScoreDataManager.ScoreData.MaxScore;
                }
                _rankUIControl.OnAnimation(scoreRate);

            }).AddTo(this);

            //クリア演出
            StageTimeController.I.OnGameClear.Subscribe(_ => _clearAnimation.PlayClearAnimation()).AddTo(this);
        }
    }
}