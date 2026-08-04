using System;

namespace Common.UI
{
    /// <summary>
    /// 確認ウィンドウのビュー
    /// </summary>
    public class ConfirmationDialogView : DialogBase
    {
        private Action _confirmAction;
        private Action _cancelAction;

        public void ShowDialog(Action confirm, Action cancel, DialogSettings settings)
        {
            _confirmAction = confirm;
            _cancelAction = cancel;
            DialogSetting(settings);
            _panelControl.OnActive();
        }

        protected override void OnCancel()
        {
            _panelControl.OnHidden();
            _cancelAction?.Invoke();
        }

        protected override void OnConfirm()
        {
            _panelControl.OnHidden();
            _confirmAction?.Invoke();
        }
    }
}