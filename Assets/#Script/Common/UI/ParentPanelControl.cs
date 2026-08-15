using System.Collections.Generic;
using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// 親子関係付きパネルコントロール
    /// </summary>
    public class ParentPanelControl : PanelControl
    {
        [SerializeField] private PanelControl[] _parentPanel;

        private Dictionary<PanelControl, bool> _parentActive = new();

        public override void OnActive(float duration = 0)
        {
            base.OnActive(duration);

            //Activeだった子パネルを表示させる
            foreach (var control in _parentPanel)
            {
                if (control == null) continue;

                if (_parentActive.TryGetValue(control, out bool active) && active)
                {
                    control.OnActive(duration);
                }
            }
        }

        public override void OnHidden(float duration = 0)
        {
            base.OnHidden(duration);

            //子パネルのActiveを保存し非表示にする
            foreach (var control in _parentPanel)
            {
                if (control == null) continue;

                _parentActive[control] = control.IsActive;
                control.OnHidden(duration);
            }
        }
    }
}