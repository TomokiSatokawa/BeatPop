using System.Collections.Generic;
using Common.BeatUpdate;
using R3;
using UnityEngine;

namespace InGame.Stage
{
    public class StageEffectSystem : MonoBehaviour
    {
        [SerializeField] private LightGroupManager _frontUpperPanelLight;
        [SerializeField] private LightGroupManager _backUpperPanelLight;
        [SerializeField] private LightGroupManager _stageGradientLight;
        [SerializeField] private LightGroupManager _laneGradientLight;
        [SerializeField] private float _startOffset;

        private IReadOnlyList<LightPatternBaseData> _patternList;
        private int _nextPatternIndex;

        private void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ => LoadPatternData()).AddTo(this);

            StageTimeController.I.IsPlaying
                .Where(x => x)
                .Subscribe(_ => UpdateNextPattern())
                .AddTo(this);


            BeatUpdateManager.FastBeatUpdate.Subscribe(32, _startOffset, _ => NextPattern());
        }

        public void NextPattern()
        {
            if (_patternList == null || _patternList.Count == 0)
                return;

            float currentTime = StageTimeController.StageTime;

            while (_nextPatternIndex < _patternList.Count)
            {
                var data = _patternList[_nextPatternIndex];

                if (data.Time > currentTime)
                {
                    break;
                }

                ApplyPattern(data);
                _nextPatternIndex++;
            }
        }


        public void ApplyPattern(LightPatternBaseData data)
        {
            switch (data.Channel)
            {
                case 0:
                    _frontUpperPanelLight.ChangePattern(data);
                    break;
                case 1:
                    _backUpperPanelLight.ChangePattern(data);
                    break;
                case 2:
                    _stageGradientLight.ChangePattern(data);
                    _laneGradientLight.ChangePattern(data);
                    break;
            }
        }

        public void UpdateNextPattern()
        {
            if (_patternList == null || _patternList.Count == 0)
                return;

            if (EditorLightData.I != null)
            {
                _patternList = EditorLightData.I.LightData;
            }

            float currentTime = StageTimeController.StageTime + _startOffset;

            _nextPatternIndex = 0;

            while (_nextPatternIndex < _patternList.Count &&
                   _patternList[_nextPatternIndex].Time <= currentTime)
            {
                _nextPatternIndex++;
            }

            // 現在有効なパターンを復元
            if (_nextPatternIndex > 0)
            {
                ApplyPattern(_patternList[_nextPatternIndex - 1]);
            }
        }
        private void LoadPatternData()
        {
            if (EditorLightData.I != null)
            {
                _patternList = EditorLightData.I.LightData;
            }
            if (InGameFileLoad.I != null)
            {
                _patternList = InGameFileLoad.I.OnStageFileLoaded.CurrentValue.LightData;
            }
        }
    }
}