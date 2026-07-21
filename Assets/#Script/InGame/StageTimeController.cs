using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using R3;
using Sound;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// StageTimeを管理する
    /// </summary>
    public class StageTimeController : SingletonMonoBehaviour<StageTimeController>
    {
        [SerializeField] private float _waitSeconds;
        [SerializeField] private float _resultDelay;
        [SerializeField] private float _timeOffset;
        [SerializeField] private float _bpm;

        public float BPM => _bpm;
        public float StartSectionTime { get; private set; }
        public float EndTime { get; private set; }
        public AudioClip SongClip { get; private set; }
        public static float StageTime { get; private set; } = float.MinValue;

        private readonly ReactiveProperty<bool> _isPlaying = new();
        public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;
        public bool IsInitialized { get; private set; } = false;

        private double _startDspTime;

        //イベント
        private readonly Subject<Unit> _onInitialized = new();
        private readonly Subject<Unit> _onGameClear = new();
        public Observable<Unit> OnInitialized => _onInitialized;
        public Observable<Unit> OnGameClear => _onGameClear;


        public void Start()
        {
            IsInitialized = false;
        }
        public void SetPlayData(NodeSaveData fileData)
        {
            float bpm  = fileData.BPM;

            float endTime = 0;
            if (fileData.Nodes.Count > 0)
            {
                endTime = fileData.Nodes[fileData.Nodes.Count - 1].Time;
            }

            int sectionIndex = Mathf.Clamp(SongPlayContext.I.StartSection, 0, fileData.Section.Count - 1);
            float sectionTime = 0;

            if (sectionIndex >= 0)
            {
                sectionTime = fileData.Section[sectionIndex];
            }

            SetPlayData(bpm, endTime, sectionTime);
        }

        public void SetPlayData(float bpm,float endTime,float startSection)
        {
            _bpm = bpm;
            EndTime = endTime;
            SongClip = SongPlayContext.I.SongData.SongData.Audio;
            _timeOffset = SongPlayContext.I.SongData.SongData.StageTimeOffSet;

            StartSectionTime = startSection;
        }

        public async UniTask SongLoadAsync()
        {
            await SoundManager.I.LoadAudioClipAsync(SongClip);
        }

        public void StartSongPlay()
        {
            float dray = Mathf.Max(0, _waitSeconds - StartSectionTime);
            float time = StartSectionTime - (_waitSeconds - dray);

            SoundManager.BGM.PlayDspBGM(SongClip, dray, time);
            _startDspTime = AudioSettings.dspTime + _waitSeconds - StartSectionTime;
            StageTime = -_waitSeconds;
            _isPlaying.Value = true;
            _onInitialized.OnNext(Unit.Default);
            IsInitialized = true;
        }

        public void UpdateStageTime()
        {
            if (!_isPlaying.CurrentValue) return;
            
            StageTime = (float)(AudioSettings.dspTime - _startDspTime) + _timeOffset;

            if (StageTime >= EndTime + _resultDelay)
            {
                //_isPlaying.Value = false;
                _onGameClear.OnNext(Unit.Default);
            }
        }

        public void ReStart()
        {
            _startDspTime = AudioSettings.dspTime - (StageTime - _timeOffset);
            _isPlaying.Value = true;
        }

        public void Pause()
        {
            _isPlaying.Value = false;
        }

        public void MoveStageTime(float amount)
        {
            StageTime += amount;
        }
    }
}