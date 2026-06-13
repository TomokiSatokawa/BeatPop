using InGame.Node;
using R3;
using UnityEngine;

public class InGameUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementView _judgementView;
    [SerializeField] private ComboUIControl _comboUIControl;
    [SerializeField] private NodeController _nodeController;
    [SerializeField] private ClearAnimation _clearAnimation;

    public void Start()
    {
        _nodeController.ShowJudge.Subscribe(data => _judgementView.ViewPrefab(data.Item1, data.Item2)).AddTo(this);
        ScoreManager.I.Combo.Where(x => x > 0).Subscribe(x => _comboUIControl.UpdateCombo(x)).AddTo(this);
        ScoreManager.I.Combo.Where(x => x <= 0).Subscribe(_ => _comboUIControl.HiddenUI()).AddTo(this);
        GameManager.I.OnGameClear.Subscribe(_ => _clearAnimation.OnAnimation()).AddTo(this);
    }
}
