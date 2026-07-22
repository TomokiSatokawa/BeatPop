using Editor.UI;
using InGame.Stage;
using UnityEngine;

namespace Editor
{
    [System.Serializable]
    public class LightPatternSnapState : EditorSnapStateBase
    {
        [SerializeField] private PatternSettingsControl _patternSettingsControl;
        private LightPatternBaseData _previewPattern = new LightPatternBaseData();
        public override void OnCreate(int laneIndex, double noteTime)
        {
            var data = _previewPattern.Clone();
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
            _patternSettingsControl?.ShowSettings(_previewPattern);
        }

        public override void OnExit() { }
    }

}