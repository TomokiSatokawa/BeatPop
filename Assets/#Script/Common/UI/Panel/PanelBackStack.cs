using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// 戻るスタック
    /// </summary>
    public class PanelBackStack : MonoBehaviour
    {
        [SerializeField] private PanelManager _panelManager;
        [SerializeField] private PanelControl _panelControl;

        private PanelControl _backPanel;
        public void ShowPanel(PanelControl backPanel)
        {
            _backPanel = backPanel;
            _panelManager.ChangeActivePanel(_panelControl);
        }

        public void BackPanel()
        {
            if (_backPanel == null) return;
            _panelManager.ChangeActivePanel(_backPanel);
            _backPanel = null;
        }
    }
}