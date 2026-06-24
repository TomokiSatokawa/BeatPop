using Common.UI;
using Input;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIControl : MonoBehaviour
{
    [SerializeField] private CountDown _countDown;
    [SerializeField] private PanelControl _panelControl;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _titleButton;
    void Start()
    {
        InputManager.PauseButton.Where(b => b).Subscribe(_ => ChangeActive()).AddTo(this);

        _continueButton.onClick.AddListener(() => ReStart());
        _retryButton.onClick.AddListener(() => GameManager.I.Retry());
        _titleButton.onClick.AddListener(() => GameManager.I.ReturnTitle());
    }

    public void ChangeActive()
    {
        if (_panelControl.IsActive)
        {
            ReStart();
        }
        else
        {
            //ŠJ‚­
            _panelControl.OnActive();
            GameManager.I.OnPause();
        }
    }
    public void ReStart()
    {
        _panelControl.OnHidden();
        GameManager.I.ReStartCountDown();
        _countDown.Play(GameManager.I.ReStartStage);
    }
}
