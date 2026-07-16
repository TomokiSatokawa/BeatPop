using System;
using System.Collections.Generic;
using System.Reflection;
using Common.BeatUpdate;
using UnityEngine;

namespace InGame.Stage
{
    /// <summary>
    /// パターンを動かす
    /// </summary>
    public class PanelLightManager : MonoBehaviour
    {
        [SerializeField] private LightControlBase[] _lights;

        private Dictionary<Type, LightPatternBase> _instancePattern = new();
        private LightPatternBase _currentPattern;
        private void Start ()
        {
            BeatUpdateManager.BeatUpdate.Subscribe(16, 0, x => BeatUpdate(x.Division));
        }

        public void BeatUpdate(int division)
        {
            if (_currentPattern == null)
                return;

            _currentPattern.BeatUpdate(division);
        }

        public void ChangePattern(LightPatternBaseData data)
        {
            Type type = Assembly.GetExecutingAssembly().GetType(data.PatternType);
            ChangePattern(type, data);
        }

        private void ChangePattern(Type type, LightPatternBaseData data)
        {
            //なかったら生成
            if (!_instancePattern.TryGetValue(type, out var pattern))
            {
                pattern = (LightPatternBase)Activator.CreateInstance(type);
            }

            _instancePattern[type] = pattern;
            _currentPattern?.Refresh();
            _currentPattern = pattern;
            _currentPattern.Initialize(data, _lights);
        }
    }
}