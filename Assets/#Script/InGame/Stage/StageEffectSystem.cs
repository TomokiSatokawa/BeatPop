using System.Collections.Generic;
using InGame.UI;
using R3;
using UnityEngine;

namespace InGame.Stage
{
    public class StageEffectSystem : MonoBehaviour
    {
        [SerializeField] private PanelLightManager _frontUpperPanelLight;
        [SerializeField] private PanelLightManager _backUpperPanelLight;
        [SerializeField] private float _startOffset;

        private IReadOnlyList<LightPatternBaseData> _patternList;
        private int _nextPatternIndex;

        private void Start()
        {
            if (EditorLightData.I != null)
            {
                _patternList = EditorLightData.I.LightData;
            }

            if (InGameFileLoad.I != null)
            {
                InGameFileLoad.I.OnStageFileLoaded.Skip(1).Subscribe(x =>
                {
                    _patternList = x.LightData;
                }).AddTo(this);
            }

            StageTimeController.I.IsPlaying
                .Where(x => x)
                .Subscribe(_ => UpdateNextPattern())
                .AddTo(this);

            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                BeatUpdateManager.I.AddFastBeatUpdate(new BeatUpdateHandle(64, _startOffset
                    , (_, _) => NextPattern()));
            });
        }

        public void NextPattern()
        {
            Debug.Log("Pattern");
            if (EditorLightData.I != null)
            {
                _patternList = EditorLightData.I.LightData;
            }
            if (InGameFileLoad.I != null)
            {
                _patternList = InGameFileLoad.I.OnStageFileLoaded.CurrentValue.LightData;
            }

            if (_patternList == null || _patternList.Count == 0)
                return;

            float currentTime = StageTimeController.StageTime;

            while (_nextPatternIndex < _patternList.Count)
            {
                var data = _patternList[_nextPatternIndex];

                if (data.Time > currentTime)
                    break;

                StartPattern(data);
                _nextPatternIndex++;
            }
        }

        public void StartPattern(LightPatternBaseData data)
        {
            switch (data.Channel)
            {
                case 0:
                    _frontUpperPanelLight.ChangePattern(data);
                    break;
                case 1:
                    _backUpperPanelLight.ChangePattern(data);
                    break;
            }
        }

        public void UpdateNextPattern()
        {
            if (_patternList == null || _patternList.Count == 0)
                return;

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
                StartPattern(_patternList[_nextPatternIndex - 1]);
            }
        }
    }
}