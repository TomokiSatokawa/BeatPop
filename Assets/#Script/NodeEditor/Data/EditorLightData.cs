using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using InGame.Stage;
using R3;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// エディターでのライトパターンデータを管理
    /// </summary>
    public class EditorLightData : EditorDataManagerBase<EditorLightData>
    {
        private List<LightPatternBaseData> _lightData = new();
        private readonly ReactiveProperty<StageSaveData> _loadedFile = new();
        private readonly Subject<(float time, int channel)> _onRemove = new();

        public IReadOnlyList<LightPatternBaseData> LightData => _lightData;
        public ReadOnlyReactiveProperty<StageSaveData> LoadedFile => _loadedFile;
        public Observable<(float time, int channel)> OnRemove => _onRemove;

        public void AddLightPattern(LightPatternBaseData lightData)
        {
            if (_lightData.Exists(x => Mathf.Abs(x.Time - lightData.Time) < Epsilon && x.Channel == lightData.Channel)) return;

            _lightData.Add(lightData);
            _lightData.Sort((a, b) => a.Time.CompareTo(b.Time));
        }

        public void RemoveLightPattern(float time, int channel)
        {
            var targetNode = _lightData.FindIndex(x => Mathf.Abs(x.Time - time) < Epsilon && x.Channel == channel);
            if (targetNode == -1) return;

            _onRemove.OnNext((time, channel));
            _lightData.RemoveAt(targetNode);
            _lightData.Sort((a, b) => a.Time.CompareTo(b.Time));
        }

        public LightPatternBaseData ChangeType(LightPatternBaseData lightData, Type type)
        {
            if (lightData.GetType() == type) return lightData;

            var targetIndex = _lightData.IndexOf(lightData);
            if (targetIndex == -1) return null;

            if (!typeof(LightPatternBaseData).IsAssignableFrom(type))
            {
                Debug.LogError($"{type.Name} は LightPatternBaseData を継承していません");
                return null;
            }

            var newData = (LightPatternBaseData)Activator.CreateInstance(type);

            newData.PatternType = lightData.PatternType;
            newData.Time = lightData.Time;
            newData.Channel = lightData.Channel;
            newData.Duration = lightData.Duration;
            newData.Power = lightData.Power;
            newData.MainColor = lightData.MainColor;

            _lightData[targetIndex] = newData;
            return newData;
        }

        protected override string GetSerializeJson()
        {
            return StageDataSerializer.SerializeJson(
                _lightData.ToArray(), LoadedFile.CurrentValue.SongDataIndex);
        }

        protected override void DeserializeJson(string json)
        {
            var data = StageDataSerializer.DeserializeJson(json);
            if (data == null)
            {
                Debug.LogError("ファイル読み込みエラー");
                return;
            }
            _lightData = data.LightData.OrderBy(x => x.Time).ToList();
            _loadedFile.OnNext(data);
        }

        protected override void CreateNewFile()
        {
            _loadedFile.Value = new();
            _lightData.Clear();
        }
    }
}