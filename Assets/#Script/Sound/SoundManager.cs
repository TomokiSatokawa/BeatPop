using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager I;
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource[] _laneSources;
        [SerializeField] private SoundDataBase _soundDataBase;

        public static SoundSection SE { get; private set; }
        public static SoundSection BGM { get; private set; }
        private readonly static List<SoundSection> _laneSE = new();
        public static IReadOnlyList<SoundSection> LaneSE => _laneSE;
        
        private void Awake()
        {
            I = this;

            SE = new(_seSource, _soundDataBase);
            BGM = new(_bgmSource, _soundDataBase);

            for(int i = 0; i < _laneSources.Length; i++)
            {
                _laneSE.Add(new(_laneSources[i], _soundDataBase));
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
                _audioSource.PlayScheduled(startDspTime);

                return startDspTime;
            }

            public void VolumeFade(float start, float end, float duration)
            {
                _volumeFade?.Kill();
                _audioSource.volume = start;
                _volumeFade = _audioSource.DOFade(end, duration);
            }
        }
    }
}