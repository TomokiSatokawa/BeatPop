using System.Linq;
using System.Threading;
using Common;
using Common.PlaySystem;
using Common.UI;
using Cysharp.Threading.Tasks;
using R3;
using Title.Custom;
using Title.SongSelect;
using UnityEngine;

namespace Title
{
    public class TitleManager : SingletonMonoBehaviour<TitleManager>
    {
        [SerializeField] private SongPlayLoader _songPlayLoader;
        [SerializeField] private SceneTransition _sceneLoad;
        [SerializeField] private FadeImageControl _fadeImageControl;
        private Subject<SongSelectData> _onStartPlay = new();
        public Observable<SongSelectData> OnStartPlay => _onStartPlay;

        public async void StartPlay(SongSelectData songSelectData)
        {
            _onStartPlay.OnNext(songSelectData);

            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);
            UniTask bgmFade = SongPreviewPlayer.I.WaitBGMFadeOut();

            await UniTask.WhenAll(fade, bgmFade);

            var patterns = await CustomDataLoader.I.GetAllCustomPattern();
            var usePattern = patterns.Where(x => x.IsSelect).First();
            _songPlayLoader.CreatePlayManager(songSelectData, usePattern);

            await _sceneLoad.LoadSceneAsync("InGame", new CancellationTokenSource().Token);

        }
    }

}