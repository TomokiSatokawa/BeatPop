using System;
using UnityEngine;
namespace InGame.Stage
{
    public abstract class LightPatternBase<T> where T : LightPatternBaseData
    {
        public bool IsEnabled { get;private set;  }
        public T Data { get; private set; }
        protected LightControlBase[] _lights;
        public virtual void Initialize(T data, LightControlBase[] lights)
        {
            Data = data;
            _lights = lights;

            foreach (var light in _lights)
            {
                light.SetColor(data.Color);
            }
        }
        public abstract void BeatUpdate(int division);

        public void ChangeEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public void Test()
        {
            foreach(var light in _lights)
            {
                light.Refresh();
            }
        }
    }
    [System.Serializable]
    public class LightPatternBaseData
    {
        public string PatternType = typeof(AlternateLightPattern).FullName;
        public float Time;
        public int Channel;
        public int Division;
        public float Duration;
        public float Power;
        public Color Color;
    }
}