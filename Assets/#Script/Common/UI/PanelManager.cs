using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// パネル切り替えのマネージャー
    /// </summary>
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private PanelControl _fastActive;
        [SerializeField] private float _fadeDuration;

        private PanelControl _currentActive;
        private void Start()
        {
            ChangeActivePanel(_fastActive);
        }

        public void ChangeActivePanel(PanelControl panel)
        {
            if (panel == null || _currentActive == panel)
                return;

            _currentActive?.OnHidden(_fadeDuration);
            panel.OnActive(_fadeDuration);
            _currentActive = panel;
        }
    }
}