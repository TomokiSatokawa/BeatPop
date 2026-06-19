using UnityEngine;
namespace Common.UI
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private PanelControl _fastActive;

        private PanelControl _currentActive;
        private void Start()
        {
            ChangeActivePanel(_fastActive);
        }

        public void ChangeActivePanel(PanelControl panel)
        {
            _currentActive?.OnHidden();
            panel?.OnActive();
            _currentActive = panel;
        }
    }
}