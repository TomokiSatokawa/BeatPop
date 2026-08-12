using System;
using Common.UI;
using Title.PlayerData;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// タイトル専用プレゼンター
    /// </summary>
    public class TitlePresenter : MonoBehaviour
    {
        [SerializeField] private PlayerInfoView[] _playerInfoView;
        [SerializeField] private InputDialogView _inputDialogView;
        [SerializeField] private ConfirmationDialogView _confirmationDialog;

        private void Start()
        {
            UpdateInfo();   

            if (string.IsNullOrWhiteSpace(PlayerDataLoader.Info.Name))
            {
                //ダイヤログを表示
                var dialogSetting = new DialogSettings(title: "プレイヤー名",showCancel: false);
                var inputSetting = new InputFieldSettings("");

                Action<string> confirmAction = PlayerDataLoader.Info.UpdateName;
                confirmAction += _ => UpdateInfo();

                _inputDialogView.ShowDialog(confirmAction, null, x => PlayerNameValidator.IsValid(x), dialogSetting, inputSetting);
            }
        }

        public void ChangePlayerName()
        {
            var dialogSetting = new DialogSettings(title: "プレイヤー名");
            var inputSetting = new InputFieldSettings(PlayerDataLoader.Info.Name);

            Action<string> confirmAction = PlayerDataLoader.Info.UpdateName;
            confirmAction += _ => UpdateInfo();

            _inputDialogView.ShowDialog(confirmAction, null, x => PlayerNameValidator.IsValid(x), dialogSetting, inputSetting);
        }

        public void DeleteSaveData()
        {
            var dialogSetting = new DialogSettings(title: "セーブデータを削除しますか？",main:"この操作は取り消せません",confirmButton:"削除");
            _confirmationDialog.ShowDialog(TitleManager.I.DeleteSaveData, null, dialogSetting);
        }

        private void UpdateInfo()
        {
            foreach(var info in _playerInfoView)
            {
                info.UpdateView();
            }
        }
    }
}