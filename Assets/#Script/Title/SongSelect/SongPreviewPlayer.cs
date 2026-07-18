using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
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

        private void Start()
        {
            TitleManager.I.OnStartPlay.Subscribe(_ =>
            {
                CancelToken();
                SoundManager.BGM.VolumeFade(0, _fadeInDuration);
            }).AddTo(this);
        }
        public void PlayPreview(IReadOnlySongData songData)
        {
            CancelToken();
            _cancellation = new CancellationTokenSource();

            WaitPlayPreviewAsync(songData.Audio, _cancellation.Token).Forget();
        }

        private void CancelToken()
        {
            if (_cancellation != null)
            {
                _cancellation.Cancel();
                _cancellation.Dispose();
                _cancellation = null;
            }
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
                UniTask loadAudio = SoundManager.I.LoadAudioClipAsync(audio);

                await UniTask.WhenAll(waitTask, loadAudio);

                if (_playAudio != null)
                {
                    SoundManager.BGMSub.PlayBGM(_playAudio, SoundManager.BGM.GetVolume(), SoundManager.BGM.GetTime());
                    SoundManager.BGMSub.VolumeFade(0, _fadeInDuration / 2);
                }

                _playAudio = audio;
                SoundManager.BGM.PlayBGM(_playAudio, 0);
            }
            SoundManager.BGM.VolumeFade(1, _fadeInDuration);

        }

        public void StopPreview()
        {
            if (SongInfoControl.I.CurrentData.HasValue
                && SongInfoControl.I.CurrentData.Value.SongData.Audio == _playAudio)
                return;

            CancelToken();

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