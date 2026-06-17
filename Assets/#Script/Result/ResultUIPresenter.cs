using UnityEngine;

public class ResultUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementCountControl _judgementCount;
    public void Start()
    {
        _judgementCount.OnAnimation(ScoreManager.I.JudgeData);
    }

}
