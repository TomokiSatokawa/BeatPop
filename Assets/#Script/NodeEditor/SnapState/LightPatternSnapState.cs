using Editor.UI;
using InGame.Stage;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// ライトパターンステート
    /// </summary>
    [System.Serializable]
    public class LightPatternSnapState : EditorSnapStateBase
    {
        [SerializeField] private PatternSettingsControl _patternSettingsControl;
        private LightPatternBaseData _editingPattern  = new LightPatternBaseData();
        public override void OnCreate(int laneIndex, double noteTime)
        {
            var data = _editingPattern .Clone();
            data.Time = (float)noteTime;
            data.Channel = laneIndex;
            EditorLightData.I.AddLightPattern(data);
        }

        public override void OnDelete(int laneIndex, double noteTime)
        {
            EditorLightData.I.RemoveLightPattern((float)noteTime, laneIndex);
        }

        public override void OnEnter()
        {
            _patternSettingsControl?.ShowSettings(_editingPattern );
        }

        public override void OnExit() { }
    }

}