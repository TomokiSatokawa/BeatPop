using InGame.Score;
using Result.UI;
using UnityEngine;

public class ResultUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementCountControl _judgementCount;
    [SerializeField] private ScoreUIControl _scoreUIControl;
    [SerializeField] private RankUIControl _rankUIControl;
    public void Start()
    {
        _judgementCount.OnAnimation(ScoreDataManager.JudgementRecorder.JudgeDataCount);
        int score = ScoreDataManager.ScoreData.Score.CurrentValue;
        int maxScore = ScoreDataManager.ScoreData.MaxScore;
        _scoreUIControl.OnAnimation(score, maxScore);
        _rankUIControl.OnAnimation(score / (float)maxScore);
        Debug.Log("maxScore" + maxScore);
    }
}
