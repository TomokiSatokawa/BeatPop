using System.Linq;
using Common.PlaySystem;
using Common;
using R3;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Title.SongSelect;
using Title.Custom;
using Common.UI;
using System.Threading;
using Sound;

namespace Title
{
    public class TitleManager : SingletonMonoBehaviour<TitleManager>
    {
        [SerializeField] private SongPlayLoader _songPlayLoader;
        [SerializeField] private SceneTransition _sceneLoad;
        [SerializeField] private FadeImageControl _fadeImageControl;
        private Subject<SongSelectData> _onStartPlay = new();
        public Observable<SongSelectData> OnStartPlay  => _onStartPlay;

        public async void StartPlay(SongSelectData songSelectData)
        {
            _onStartPlay.OnNext(songSelectData);

            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);
            UniTask bgmFade = SongPreviewPlayer.I.WaitBGMFadeOut();

            await UniTask.WhenAll(fade, bgmFade);

            var patterns = await CustomDataLoader.I.GetCustomPattern();
            var usePattern = patterns.Where(x => x.IsSelect).First();
            _songPlayLoader.CreatePlayManager(songSelectData, usePattern);

            await _sceneLoad.LoadSceneAsync("InGame", new CancellationTokenSource().Token);

        }
    }

}