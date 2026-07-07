using InGame.UI;
using R3;
using UnityEngine;

namespace InGame.Stage
{

    public class PanelLightManager : MonoBehaviour
    {
        [SerializeField] private LightPatternBaseData _baseData;
        [SerializeField] private float _duration;
        [SerializeField] private LightControlBase[] _lights;

        private LightPatternBase<LightPatternBaseData> _currentPattern;
        private void Start ()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                BeatUpdateManager.I.Register(new BeatUpdateHandle(64, 0, (_,division) => BeatUpdate(division)));
            }).AddTo(this);

            _currentPattern = new BeatSyncLightPattern(_baseData,_lights);
        }

        public void BeatUpdate(int division)
        {
            _currentPattern.BeatUpdate(division);
        }
    }
}