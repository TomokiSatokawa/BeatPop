using System;
using Newtonsoft.Json;
using UnityEngine;
namespace InGame.Stage
{
    public abstract class LightPatternBase
    {
        public abstract void Initialize(LightPatternBaseData data, LightControlBase[] lights);
        public abstract void BeatUpdate(int division);
        public abstract void Refresh();
        public abstract LightPatternBaseData GetData();
    }
    public abstract class LightPatternBase<T>: LightPatternBase where T : LightPatternBaseData
    {
        public bool IsEnabled { get;private set;  }
        public T Data { get; private set; }
        protected LightControlBase[] _lights;

        public override void Initialize(LightPatternBaseData data, LightControlBase[] lights)
        {
            InitializeCore(DataCast(data), lights);
        }
        public virtual void InitializeCore(T data, LightControlBase[] lights)
        {
            Data = data;
            _lights = lights;

            foreach (var light in _lights)
            {
                light.SetColor(data.Color);
            }
        }

        public void ChangeEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public override void Refresh()
        {
            foreach(var light in _lights)
            {
                light.Refresh();
            }
        }

        public override LightPatternBaseData GetData()
        {
            return Data;
        }

        private T DataCast(LightPatternBaseData data)
        {
            if (data is not T tData)
            {
                Debug.LogError($"{data.GetType()} は {typeof(T)} にキャストできません。");
                return null;
            }
                return tData;
        }
    }
    
    [Serializable]
    public class LightPatternBaseData
    {
        public string PatternType = typeof(AlternateLightPattern).FullName;
        public float Time;
        public int Channel;
        public int Division;
        public float Duration;
        public float Power;


        [JsonIgnore]
        public Color Color;

        public float R;
        public float G;
        public float B;
        public float A;
    }
}   