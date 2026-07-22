using System.Collections.Generic;
using Common.PlaySystem;
using R3;
using Sound;
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
        private readonly Dictionary<PoolPrefabType, SESoundType> _nodeSE = new();
        public IReadOnlyDictionary<PoolPrefabType, SESoundType> NodeSE => _nodeSE;

        void Start()
        {
            StageTimeController.I.OnInitialized.Subscribe(_ => Initialize()).AddTo(this);
        }

        private void Initialize()
        {
            CustomSoundPattern soundPattern = SongPlayContext.I?.PatternData?.SoundPattern ?? _soundData.GetDefaultCustom();

            _nodeSE.Add(PoolPrefabType.NormalNote, _soundData.TapSE[soundPattern.NormalSE].Value);
            _nodeSE.Add(PoolPrefabType.FlickNote, _soundData.TapSE[soundPattern.FlickSE].Value);
            _nodeSE.Add(PoolPrefabType.HoldNoteStart, _soundData.TapSE[soundPattern.HoldStart].Value);
            _nodeSE.Add(PoolPrefabType.HoldNoteFill, _soundData.TapSE[soundPattern.HoldFill].Value);
            _nodeSE.Add(PoolPrefabType.HoldNoteEnd, _soundData.TapSE[soundPattern.HoldEnd].Value);
        }
    }
}