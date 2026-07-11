using System;
using System.Collections.Generic;
using System.Reflection;
using InGame.UI;
using R3;
using UnityEngine;

namespace InGame.Stage
{
    /// <summary>
    /// パターンを動かす
    /// </summary>
    public class PanelLightManager : MonoBehaviour
    {
        [SerializeField] public LightPatternBaseData data;
        [SerializeField] private LightControlBase[] _lights;

        private Dictionary<Type, LightPatternBase<LightPatternBaseData>> _instancePattern = new();
        private LightPatternBase<LightPatternBaseData> _currentPattern;
        private void Start ()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ =>
            {
                BeatUpdateManager.I.Register(new BeatUpdateHandle(64, 0, (_,division) => BeatUpdate(division)));
            }).AddTo(this);

        }

        public void BeatUpdate(int division)
        {
            _currentPattern?.BeatUpdate(division);
        }

        public void ChangePattern(LightPatternBaseData data)
        {
            Type type = Assembly.GetExecutingAssembly().GetType(data.PatternType);
            Debug.Log(type);
            ChangePattern(type, data);
        }

        private void ChangePattern(Type type, LightPatternBaseData data)
        {
            if (!_instancePattern.TryGetValue(type, out var pattern))
            {
                pattern = (LightPatternBase<LightPatternBaseData>)Activator.CreateInstance(type);
            }

            Debug.Log("ChangePattern");
            _instancePattern[type] = pattern;
            _currentPattern?.Test();
            _currentPattern = pattern;
            _currentPattern.Initialize(data, _lights);
        }
    }
}