using System.Threading;
using Cysharp.Threading.Tasks;
using Sound;
using UnityEngine;

namespace Title.SongSelect
{
    public class SongPreviewPlayer : SingletonMonoBehaviour<SongPreviewPlayer>
    {
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _playWaitTime;
        [SerializeField] private float _stopWaitTime;

        private AudioClip _playAudio;
        private CancellationTokenSource _cancellation;
        public void PlayPreview(IReadOnlySongData songData)
        {
            if (_cancellation != null)
            {
                _cancellation.Cancel();
                _cancellation.Dispose();
                _cancellation = null;
            }
            _cancellation = new CancellationTokenSource();

            WaitPlayPreviewAsync(songData.Audio, _cancellation.Token).Forget();
        }

        private async UniTask WaitPlayPreviewAsync(AudioClip audio, CancellationToken token)
        {
            if (audio != _playAudio)
            {
                float waitTime = 0;
                if (_playAudio == null)
                {
                    waitTime = _playWaitTime;
                }

                UniTask waitTask = UniTask.WaitForSeconds(waitTime, cancellationToken: token);

                audio.LoadAudioData();
                UniTask loadAudio = UniTask.WaitUntil(() => audio.loadState == AudioDataLoadState.Loaded, cancellationToken: token);

                await UniTask.WhenAll(waitTask, loadAudio);

                if(_playAudio != null)
                {
                    SoundManager.BGMSub.PlayBGM(_playAudio, SoundManager.BGM.GetVolume(), SoundManager.BGM.GetTime());
                    SoundManager.BGMSub.VolumeFade(0, _fadeInDuration / 2);
                }

                _playAudio = audio;
                SoundManager.BGM.PlayBGM(_playAudio, 0);
                //SoundManager.BGM.SetVolume(0);
            }
            SoundManager.BGM.VolumeFade(1, _fadeInDuration);

        }

        public void StopPreview()
        {
            if (_cancellation != null)
            {
                _cancellation.Cancel();
                _cancellation.Dispose();
                _cancellation = null;
            }

            _cancellation = new CancellationTokenSource();

            WaitStopPreviewAsync(_cancellation.Token).Forget();
        }

        private async UniTask WaitStopPreviewAsync(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_stopWaitTime, cancellationToken: token);
            SoundManager.BGM.VolumeFade(0, _fadeInDuration);
            await UniTask.WaitForSeconds(_fadeInDuration, cancellationToken: token);
            _playAudio = null;

        }
    }
}