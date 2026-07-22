using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using R3;
using Sound;
using UnityEngine;

namespace Title.SongSelect
{
    /// <summary>
    /// SongSelect画面でのプレビューサウンド再生指示
    /// </summary>
    public class SongPreviewPlayer : SingletonMonoBehaviour<SongPreviewPlayer>
    {
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _playWaitTime;
        [SerializeField] private float _stopWaitTime;
        [SerializeField] private AudioClip _titleBGM;

        private AudioClip _playAudio;
        private float _titleBGMTime;
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
            _playAudio = audio;
            float waitTime = _playWaitTime;
            if (_playAudio != null)
            {
                waitTime = _playWaitTime / 2;
            }

            UniTask waitTask = UniTask.WaitForSeconds(waitTime, cancellationToken: token);

            if (audio == _playAudio)
            {
                var isCanceled = await waitTask.SuppressCancellationThrow();

                if (isCanceled)
                    return;
            }
            else
            {
                UniTask loadAudio = SoundManager.I.LoadAudioClipAsync(audio);

                var isCanceled = await UniTask.WhenAll(waitTask, loadAudio).SuppressCancellationThrow();

                if (isCanceled)
                    return;
            }

            FadeChangeBGM(audio);
        }

        public void FadeChangeBGM(AudioClip audio)
        {
            if (SoundManager.BGM.Audio == _titleBGM)
            {
                _titleBGMTime = SoundManager.BGM.Time;
            }

            SoundManager.CrossFadeBGM(SoundManager.BGM, SoundManager.BGMSub, audio, _fadeInDuration);
        }

        public void StopPreview()
        {
            Debug.Log("StopPreview");
            if (SongInfoControl.I.CurrentData.HasValue
                && SongInfoControl.I.CurrentData.Value.SongData.Audio == _playAudio)
                return;

            Debug.Log("Stop");
            CancelToken();

            _cancellation = new CancellationTokenSource();

            WaitStopPreviewAsync(_cancellation.Token).Forget();
        }

        private async UniTask WaitStopPreviewAsync(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_stopWaitTime, cancellationToken: token);
            SoundManager.CrossFadeBGM(SoundManager.BGM, SoundManager.BGMSub, _titleBGM, _fadeInDuration, _titleBGMTime, true);
            await UniTask.WaitForSeconds(_fadeInDuration, cancellationToken: token);
            _playAudio = _titleBGM;
        }
    }
}