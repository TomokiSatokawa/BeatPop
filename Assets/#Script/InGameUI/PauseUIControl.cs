using Common.UI;
using Input;
using R3;
using UnityEngine;

public class PauseUIControl : MonoBehaviour
{
    [SerializeField] private PanelControl _panelControl;
    void Start()
    {
        InputManager.PauseButton.Where(b => b).Subscribe(_ => ChangeActive()).AddTo(this);
    }

    public void ChangeActive()
    {
        if (_panelControl.IsActive)
        {
            //•Â‚¶‚é
            _panelControl.OnHidden();
            GameManager.I.ReStart();
        }
        else
        {
            //ŠJ‚­
            _panelControl.OnActive();
            GameManager.I.OnPause();
        }
    }
}
