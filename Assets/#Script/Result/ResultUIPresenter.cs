using Result.UI;
using UnityEngine;

public class ResultUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementCountControl _judgementCount;
    [SerializeField] private ScoreUIControl _scoreUIControl;
    public void Start()
    {
        _judgementCount.OnAnimation(ScoreManager.I.JudgeData);
        ScoreManager.I.GetSumScore(out int score, out int max);
        _scoreUIControl.OnAnimation(score, max);
    }
}
