using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// パネル切り替えのマネージャー
    /// </summary>
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private PanelControl _startActive;
        [SerializeField] private float _fadeDuration;

        private PanelControl _currentActive;
        public PanelControl CurrentActive => _currentActive;

        private void Start()
        {
            ChangeActivePanel(_startActive);
        }

        public void ChangeActivePanel(PanelControl panel)
        {
            ChangeActivePanel(panel, _fadeDuration);
        }

        public void ChangeActivePanel(PanelControl panel, float fadeDuration)
        {
            if (panel == null || _currentActive == panel)
                return;

            _currentActive?.OnHidden(fadeDuration);
            panel.OnActive(fadeDuration);
            _currentActive = panel;
        }
    }
}