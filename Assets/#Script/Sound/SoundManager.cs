using UnityEngine;

namespace Sound
{
    public class SoundManager :MonoBehaviour
    {
        public static SoundManager I;
        [SerializeField] private AudioSource _seAudio;
        [SerializeField] private AudioSource _seBGM;
        [SerializeField] private SoundDataBase _soundDataBase;
        private void Awake()
        {
            I = this;
        }

        public void PlaySESound(SESoundType type)
        {
            var seData = _soundDataBase.GetSESound(type);
            _seAudio.PlayOneShot(seData.Clip, seData.Volume);
        }

        public double PlayBGMSound(AudioClip clip, float dray)
        {
            double startDspTime;

            startDspTime = AudioSettings.dspTime + dray;

            _seBGM.clip = clip;
            _seBGM.PlayScheduled(startDspTime);

            return startDspTime;
        }

        public void IsPause(bool stop)
        {
            if (stop)
            {
                _seBGM.Pause();
            }
            else
            {
                _seBGM.UnPause();
            }
        }
    }
}