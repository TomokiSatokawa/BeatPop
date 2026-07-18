using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Sound
{
    public class SoundManager : SingletonPersistent<SoundManager>
    {
        private const float MaxAudioLoadTimePerFrameMs = 0.5f;
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _bgmSubSource;
        [SerializeField] private AudioSource[] _laneSources;
        [SerializeField] private SoundDataBase _soundDataBase;

        public static SoundSection SE { get; private set; }
        public static SoundSection BGM { get; private set; }
        public static SoundSection BGMSub { get; private set; }
        private readonly static List<SoundSection> _laneSE = new();
        public static IReadOnlyList<SoundSection> LaneSE => _laneSE;

        public void Start()
        {
            SE = new(_seSource, _soundDataBase);
            BGM = new(_bgmSource, _soundDataBase);
            BGMSub = new(_bgmSubSource, _soundDataBase);

            for (int i = 0; i < _laneSources.Length; i++)
            {
                _laneSE.Add(new(_laneSources[i], _soundDataBase));
            }
        }

        public async UniTask LoadAudioClipAsync(AudioClip clip)
        {
            if (clip == null)
                return;

            if (clip.loadState == AudioDataLoadState.Loaded)
                return;

            var sw = System.Diagnostics.Stopwatch.StartNew();

            clip.LoadAudioData();

            while (clip.loadState == AudioDataLoadState.Loading)
            {
                if (sw.ElapsedMilliseconds >= MaxAudioLoadTimePerFrameMs)
                {
                    sw.Restart();
                    await UniTask.Yield();
                }
            }

            if (clip.loadState != AudioDataLoadState.Loaded)
            {
                Debug.LogError($"AudioClipÇÃì«Ç›çûÇ›Ç…é∏îsÇµÇ‹ÇµÇΩ : {clip.name}");
            }
        }

        public class SoundSection
        {
            private readonly AudioSource _audioSource;
            private readonly SoundDataBase _soundDataBase;

            private Tween _volumeFade;

            public SoundSection(AudioSource audioSource, SoundDataBase soundDataBase)
            {
                _audioSource = audioSource;
                _soundDataBase = soundDataBase;
            }

            public void PlaySE(SESoundType type, float volume = 1f)
            {
                var soundData = _soundDataBase.GetSESound(type);
                PlaySE(soundData.Clip, volume * soundData.Volume);
            }

            public void PlaySE(AudioClip clip ,float volume = 1f)
            {
                _audioSource.PlayOneShot(clip,volume);
            }

            public void PlayBGM(SESoundType type, float volume = 1f,float time = 0, bool isLoop = false)
            {
                var soundData = _soundDataBase.GetSESound(type);
                PlayBGM(soundData.Clip, volume * soundData.Volume,time,isLoop);
            }

            public void PlayBGM(AudioClip clip,float volume = 1f, float time = 0, bool isLoop = false)
            {
                _volumeFade?.Kill();

                _audioSource.clip = clip;
                _audioSource.loop = isLoop;
                _audioSource.volume = volume;
                _audioSource.time = time;
                _audioSource.Play();
            }

            public void StopBGM()
            {
                _audioSource.Stop();
            }

            public void IsPause(bool stop)
            {
                if (stop)
                {
                    _audioSource.Pause();
                }
                else
                {
                    _audioSource.UnPause();
                }
            }

            public double PlayDspBGM(AudioClip clip, float dray, float time)
            {
                _volumeFade?.Kill();
                double startDspTime;

                startDspTime = AudioSettings.dspTime + dray;

                _audioSource.clip = clip;
                _audioSource.time = time;
                SetVolume(1f);
                _audioSource.PlayScheduled(startDspTime);

                return startDspTime;
            }

            public void SetVolume(float volume)
            {
                _audioSource.volume = volume;
            }

            public void VolumeFade(float end, float duration)
            {
                _volumeFade?.Kill();
                _volumeFade = _audioSource.DOFade(end, duration);
            }

            public float GetTime()
            {
                return _audioSource.time;
            }
            
            public float GetVolume()
            {
                return _audioSource.volume;
            }
        }
    }
}