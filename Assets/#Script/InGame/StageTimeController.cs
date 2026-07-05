using Common.PlaySystem;
using Cysharp.Threading.Tasks;
using R3;
using Sound;
using UnityEngine;

namespace InGame.UI
{

    public class StageTimeController:SingletonMonoBehaviour<StageTimeController>
    {
        [SerializeField] private float _waitSeconds;
        [SerializeField] private float _resultDelay;

        public float BPM { get; private set; }
        public float StartSectionTime { get;private set; }
        public float EndTime { get; private set; }
        public AudioClip SongClip { get; private set; }
        public static float StageTime { get; private set; } = float.MinValue;
        public static bool IsPlaying { get; private set; } = false;


        private Subject<Unit> _onGameClear = new();
        public Observable<Unit> OnGameClear => _onGameClear;


        private double _startDspTime;

        public void SetPlayData(NodeSaveData fileData)
        {
            BPM = fileData.BPM;
            EndTime = fileData.Nodes[fileData.Nodes.Count - 1].Time;
            SongClip = SongPlayManager.I.SongData.SongData.Audio;

            int sectionIndex = Mathf.Clamp(SongPlayManager.I.StartSection, 0, fileData.Section.Count) - 1;
            if (sectionIndex == -1)
            {
                StartSectionTime = 0;
            }
            else
            {
                StartSectionTime = fileData.Section[SongPlayManager.I.StartSection];
            }
        }

        public async UniTask SongLoadAsync()
        {
            SongClip.LoadAudioData();
            await UniTask.WaitUntil(() => SongClip.loadState == AudioDataLoadState.Loaded);
        }

        public void StartSongPlay()
        {
            float dray = Mathf.Max(0, _waitSeconds - StartSectionTime);
            float time = StartSectionTime - (_waitSeconds - dray);

            SoundManager.I.PlayBGMSound(SongClip, dray, time);
            _startDspTime = AudioSettings.dspTime + _waitSeconds - StartSectionTime;
            StageTime = -_waitSeconds;
            IsPlaying = true;
        }

        public void UpdateStageTime()
        {
            if (!IsPlaying) return;

            float offset = SongPlayManager.I.SongData.SongData.StageTimeOffSet;
            StageTime = (float)(AudioSettings.dspTime - _startDspTime) + offset;

            if (StageTime >= EndTime + _resultDelay)
            {
                IsPlaying = false;
                _onGameClear.OnNext(Unit.Default);
            }
        }

        public void ReStart()
        {
            float offset = SongPlayManager.I.SongData.SongData.StageTimeOffSet;
            _startDspTime = AudioSettings.dspTime - (StageTime - offset);
            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }
    }
}