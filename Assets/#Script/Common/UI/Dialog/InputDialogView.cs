using System;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// ì¸óÕÉ_ÉCÉÑÉçÉOÇÃView
    /// </summary>
    public class InputDialogView : DialogBase
    {
        [SerializeField] private TMP_InputField _inputField;

        private Action<string> _confirmAction;
        private Action _cancelAction;
        private Func<string, string> _checkFunc;

        public void ShowDialog(Action<string> confirm, Action cancel, Func<string, string> check, DialogSettings DialogSettings,InputFieldSettings inputSettings)
        {
            _panelControl.OnActive();

            _confirmAction = confirm;
            _cancelAction = cancel;
            _checkFunc = check;

            DialogSetting(DialogSettings);
        }

        protected override void OnCancel()
        {
            _cancelAction?.Invoke();
            _panelControl.OnHidden();
        }

        protected override void OnConfirm()
        {
            string inputValue = _inputField.text;

            string checkResult = _checkFunc?.Invoke(inputValue) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(checkResult))
            {
                Debug.LogError(checkResult);
                return;
            }

            _panelControl.OnHidden();
            _confirmAction?.Invoke(inputValue);
        }
    }

    public ref struct InputFieldSettings
    {
        public string DefaultValue { get; }
        public InputFieldSettings(string defaultValue = "")
        {
            DefaultValue = defaultValue;
        }

    }
}