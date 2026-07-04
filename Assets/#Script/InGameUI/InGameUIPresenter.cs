using Common.PlaySystem;
using InGame.Node;
using InGame.UI;
using R3;
using UnityEngine;

public class InGameUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementView _judgementView;
    [SerializeField] private ComboUIControl _comboUIControl;
    [SerializeField] private NodeController _nodeController;
    [SerializeField] private ClearAnimation _clearAnimation;
    [SerializeField] private ScoreUIView _scoreUIView;
    [SerializeField] private MissAnimation _missAnimation;

    public void Start()
    {
        _scoreUIView.SetData(SongPlayManager.I.SongData.SongData.MaxScore);

        _nodeController.ShowJudge.Subscribe(data => _judgementView.ViewPrefab(data.Item1, data.Item2)).AddTo(this);

        ScoreManager.I.Combo.Where(x => x > 0).Subscribe(x => _comboUIControl.UpdateCombo(x)).AddTo(this);
        ScoreManager.I.Combo.Where(x => x <= 0).Subscribe(_ =>
        {
            _comboUIControl.HiddenUI();
            _missAnimation.PlayAnimation();
        }).AddTo(this);
        ScoreManager.I.Score.Subscribe(x => _scoreUIView.UpdateScore(x)).AddTo(this);

        GameManager.I.OnGameClear.Subscribe(_ => _clearAnimation.PlayClearAnimation()).AddTo(this);
    }
}
