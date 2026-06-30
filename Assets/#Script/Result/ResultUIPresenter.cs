using Result.UI;
using UnityEngine;

public class ResultUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementCountControl _judgementCount;
    [SerializeField] private ScoreUIControl _scoreUIControl;
    [SerializeField] private RankUIControl _rankUIControl;
    public void Start()
    {
        _judgementCount.OnAnimation(ScoreManager.I.JudgeDataCount);
        ScoreManager.I.GetSumScore(out int score);
        _scoreUIControl.OnAnimation(score, ScoreManager.I.MaxPossibleScore);
        _rankUIControl.OnAnimation(score / (float)ScoreManager.I.MaxPossibleScore);
    }
}
