using Common;
using Common.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title.Custom
{
    /// <summary>
    /// パターンのオプションメニュー
    /// </summary>
    public class PatternOptionMenuControl : MonoBehaviour
    {
        [SerializeField] private PanelControl _panelControl;
        [SerializeField] private InputDialogView _inputDialog;
        [SerializeField] private ConfirmationDialogView _confirmationDialog;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Button _rename;
        [SerializeField] private Button _setPattern;
        [SerializeField] private Button _delete;
        [SerializeField] private Vector2 _offSet;
        [SerializeField] private FloatRange _showRangeY;
        [SerializeField] private UnityEvent<PatternJsonData,string> _onRename;
        [SerializeField] private UnityEvent<PatternJsonData> _onSetPattern;
        [SerializeField] private UnityEvent<PatternJsonData> _onDelete;

        private PatternJsonData _patternJsonData;

        private void Start()
        {
            _rename.onClick.AddListener(OnRename);
            _setPattern.onClick.AddListener(OnSetPattern);
            _delete.onClick.AddListener(OnDelete);
        }

        public void Open(Vector2 openButtonPos,PatternJsonData pattern)
        {
            Debug.Log(openButtonPos);
            Vector2 pos = openButtonPos + _offSet;

            //範囲外だった場合OffSetYを反転
            if (!_showRangeY.Contains(pos.y))
            {
                pos.y = openButtonPos.y - _offSet.y;
            }

            _panel.transform.position = pos;
            _panelControl.OnActive();
            _patternJsonData = pattern;

            //初期パターンは削除できない
            _delete.interactable = !pattern.IsDefault;
        }

        private void OnRename()
        {
            var dialogSettings = new DialogSettings(title:"名前を変更");
            var input = new InputFieldSettings(_patternJsonData.PatternName);
            _inputDialog.ShowDialog(x => _onRename?.Invoke(_patternJsonData, x), null, x => "", dialogSettings,input);
            _panelControl.OnHidden();
        }

        private void OnSetPattern()
        {
            _onSetPattern?.Invoke(_patternJsonData);
            _panelControl.OnHidden();
        }
        private void OnDelete()
        {
            var dialogSettings = new DialogSettings(title: "本当に削除しますか？",confirmButton:"削除");
            _confirmationDialog.ShowDialog(() => _onDelete?.Invoke(_patternJsonData),null, dialogSettings);;
            _panelControl.OnHidden();
        }
    }
}