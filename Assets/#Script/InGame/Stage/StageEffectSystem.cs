using System.Collections.Generic;
using Common.BeatUpdate;
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
        [SerializeField] private bool _isAllCheckPattern;

        private IReadOnlyList<LightPatternBaseData> _patternList;
        private int _nextPatternIndex;

        private void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ => SetPattern()).AddTo(this);

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

                StartPattern(data);
                _nextPatternIndex++;
            }
        }
        

        public void StartPattern(LightPatternBaseData data)
        {
            switch (data.Channel)
            {
                case 0:
                    Debug.Log(data.GetType().ToString());
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
                StartPattern(_patternList[_nextPatternIndex - 1]);
            }
        }
        private void SetPattern()
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