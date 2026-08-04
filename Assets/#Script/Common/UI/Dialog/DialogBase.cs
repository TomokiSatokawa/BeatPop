using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
   /// <summary>
   /// ダイヤログUIのベース
   /// </summary>
    public　abstract class DialogBase : MonoBehaviour
    {
        [SerializeField] protected PanelControl _panelControl;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private TextMeshProUGUI _cancelText;
        [SerializeField] private TextMeshProUGUI _confirmText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _mainText;

        private void Start()
        {
            _cancelButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.AddListener(OnCancel);

            _confirmButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.AddListener(OnConfirm);
        }

        protected virtual void OnStart() { }
        protected void DialogSetting(DialogSettings settings)
        {
            _cancelText.text = settings.CancelText;
            _confirmText.text = settings.ConfirmText;

            _titleText.text = settings.TitleText;
            _mainText.text = settings.MainText;
        }

        protected abstract void OnCancel();
        protected abstract void OnConfirm();
    }

    public ref struct DialogSettings
    {
        public string TitleText { get; }
        public string MainText { get; }
        public string ConfirmText { get; }
        public string CancelText { get; }

        public DialogSettings(string title = "タイトル", string main = "", string confirmButton = "OK", string cancelButton = "キャンセル")
        {
            TitleText = title;
            MainText = main;
            ConfirmText = confirmButton;
            CancelText = cancelButton;
        }
    }
}