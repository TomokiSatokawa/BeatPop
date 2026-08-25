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
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Button _rename;
        [SerializeField] private Button _setPattern;
        [SerializeField] private Button _delete;
        [SerializeField] private Vector2 _offSet;
        [SerializeField] private FloatRange _showRangeY;
        [SerializeField] private UnityEvent<PatternJsonData> _onRename;
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
        }

        private void OnRename()
        {
            _onRename?.Invoke(_patternJsonData);
            _panelControl.OnHidden();
        }

        private void OnSetPattern()
        {
            _onSetPattern?.Invoke(_patternJsonData);
            _panelControl.OnHidden();
        }
        private void OnDelete()
        {
            _onDelete?.Invoke(_patternJsonData);
            _panelControl.OnHidden();
        }
    }
}