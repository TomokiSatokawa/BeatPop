using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using R3;
using Sound;
using Unity.VisualScripting;
using UnityEngine;

namespace Title.SongSelect
{
    /// <summary>
    /// タイトルシーン内専用SoundController
    /// </summary>
    public class TitleSoundController : SingletonMonoBehaviour<TitleSoundController>
    {
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _playWaitTime;
        [SerializeField] private float _stopWaitTime;
        [SerializeField] private AudioClip _titleBGM;

        private IReadOnlySongData _playSongData;
        private AudioClip _playAudio;
        private float _titleBGMTime;
        private CancellationTokenSource _cancellation;

        private void Start()
        {
            TitleManager.I.OnStartPlay.Subscribe(_ =>
            {
                CancelToken();
            }).AddTo(this);


            SoundManager.BGM.PlayBGM(null);
            StopPreview();
        }

        private void Update()
        {
            if (_playSongData == null) return;

            //Preview範囲を超過
            if (SoundManager.BGM.Time >= _playSongData.PreviewTime.Max)
            {
                Debug.Log("超過");
                FadeChangeBGM(_playSongData.Audio, _playSongData.PreviewTime.Min,false);
            }
        }

        public void WaitPlayPreview(IReadOnlySongData songData)
        {
            CancelToken();
            _cancellation = new CancellationTokenSource();

            WaitPlayPreviewAsync(songData, _cancellation.Token).Forget();
        }

        public void PlayPreview(IReadOnlySongData songData)
        {
            _playAudio = songData.Audio;
            _playSongData = songData;
            FadeChangeBGM(songData.Audio, songData.PreviewTime.Min);
        }

        public async UniTask WaitBGMFadeOut()
        {
            SoundManager.BGM.VolumeFade(0, _fadeInDuration);
            await UniTask.WaitForSeconds(_fadeInDuration);
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

        private async UniTask WaitPlayPreviewAsync(IReadOnlySongData songData, CancellationToken token)
        {
            AudioClip audio = songData.Audio;

            if (audio == null) return;
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

            FadeChangeBGM(audio,songData.PreviewTime.Min);
            _playSongData = songData;
        }

        public void FadeChangeBGM(AudioClip audio,float time = 0f,bool rejectSameSong = true)
        {
            if (SoundManager.BGM.Audio == _titleBGM)
            {
                _titleBGMTime = SoundManager.BGM.Time;
            }

            SoundManager.CrossFadeBGM(SoundManager.BGM, SoundManager.BGMSub, audio, _fadeInDuration,time: time , rejectSameSong: rejectSameSong);
        }

        public void StopPreview()
        {
            if (SongInfoControl.CurrentData.HasValue
                && SongInfoControl.CurrentData.Value.SongData.Audio == _playAudio)
                return;

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
            _playSongData = null;
        }
    }
}