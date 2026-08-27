using System;
using System.Collections.Generic;
using Common.PlaySystem;
using R3;
using Title.Custom;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// TitleでカスタムしたサウンドをInGameで使いやすくする
    /// </summary>
    public class InGameCustomSoundData : SingletonMonoBehaviour<InGameCustomSoundData>
    {
        [SerializeField] private CustomSoundData _soundData;
        private readonly Dictionary<PoolPrefabType, SEData> _nodeSE = new();
        public IReadOnlyDictionary<PoolPrefabType, SEData> NodeSE => _nodeSE;

        void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ => Initialize()).AddTo(this);
        }

        private void Initialize()
        {
            CustomSoundPattern soundPattern = SongPlayContext.I?.PatternData?.SoundPattern ?? _soundData.GetDefault();
            var presetData = _soundData.PresetDatas[soundPattern.GetData(CustomSoundType.Preset)].Value;

            foreach (CustomSoundType type in Enum.GetValues(typeof(CustomSoundType)))
            {
                if (type == CustomSoundType.Preset) continue;

                int index;
                if (soundPattern.UsePreset)
                    index = presetData[type];
                else
                    index = soundPattern.GetData(type);

                switch (type)
                {
                    case CustomSoundType.Preset:
                        break;
                    case CustomSoundType.Normal:
                    case CustomSoundType.HoldStart:
                    case CustomSoundType.HoldEnd:
                    case CustomSoundType.TickNode:
                    case CustomSoundType.HighScore:
                        _nodeSE.Add(CustomSound.CustomTypeToPoolType(type), _soundData.TapSE[index]);
                        break;
                    case CustomSoundType.HoldFill:
                        _nodeSE.Add(CustomSound.CustomTypeToPoolType(type), _soundData.HoldSE[index]);
                        break;
                    case CustomSoundType.Flick:
                        _nodeSE.Add(CustomSound.CustomTypeToPoolType(type), _soundData.TapSE[index]);
                        _nodeSE.Add(PoolPrefabType.HoldFlickEnd, _soundData.TapSE[index]);
                        break;
                }
            }
        }
    }
}