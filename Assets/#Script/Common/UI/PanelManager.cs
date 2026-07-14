using UnityEngine;
namespace Common.UI
{
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
            _currentActive?.OnHidden(_fadeDuration);
            panel?.OnActive(_fadeDuration);
            _currentActive = panel;
        }
    }
}