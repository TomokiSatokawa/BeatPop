using System.Linq;
using System.Threading;
using Common;
using Common.PlaySystem;
using Common.UI;
using Cysharp.Threading.Tasks;
using R3;
using Title.Custom;
using Title.PlayerData;
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
        public async void StartPlay(SongSelectData songSelectData,bool autoPlay = false)
        {
            _onStartPlay.OnNext(songSelectData);

            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);
            UniTask bgmFade = TitleSoundController.I.WaitBGMFadeOut();

            await UniTask.WhenAll(fade, bgmFade);

            var patterns = await CustomDataLoader.I.GetAllCustomPattern();
            var usePattern = patterns.Where(x => x.IsSelect).First();
            _songPlayLoader.CreatePlayManager(songSelectData, usePattern,autoPlay);

            await _sceneLoad.LoadSceneAsync("InGame", new CancellationTokenSource().Token);
        }

        public async void DeleteSaveData()
        {
            PlayerDataLoader.DisposeSingleton();
            CustomDataLoader.DisposeSingleton();

            UniTask delete = FileStorage.DeleteAllFile();
            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);

            await UniTask.WhenAll(delete, fade);
            await _sceneLoad.LoadSceneAsync("StartScreen", new CancellationTokenSource().Token);

        }
    }

}