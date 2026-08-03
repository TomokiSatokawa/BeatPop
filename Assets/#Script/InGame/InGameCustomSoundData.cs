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
            CustomSoundPattern soundPattern = SongPlayContext.I?.PatternData?.SoundPattern ?? _soundData.GetDefaultCustom();

            _nodeSE.Add(PoolPrefabType.NormalNote, _soundData.TapSE[soundPattern.NormalSE]);
            _nodeSE.Add(PoolPrefabType.HighScoreNote, _soundData.TapSE[soundPattern.HighScore]);
            _nodeSE.Add(PoolPrefabType.FlickNote, _soundData.TapSE[soundPattern.FlickSE]);
            _nodeSE.Add(PoolPrefabType.HoldNoteStart, _soundData.TapSE[soundPattern.HoldStart]);
            _nodeSE.Add(PoolPrefabType.HoldNoteFill, _soundData.HoldSE[soundPattern.HoldFill]);
            _nodeSE.Add(PoolPrefabType.HoldFlickEnd, _soundData.TapSE[soundPattern.FlickSE]);
            _nodeSE.Add(PoolPrefabType.HoldNoteEnd, _soundData.TapSE[soundPattern.HoldEnd]);
            _nodeSE.Add(PoolPrefabType.TickNode, _soundData.TapSE[soundPattern.TickNode]);
        }
    }
}