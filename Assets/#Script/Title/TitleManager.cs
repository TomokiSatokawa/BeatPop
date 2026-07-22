using System.Linq;
using Common.PlaySystem;
using Common;
using R3;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Title.SongSelect;
using Title.Custom;

namespace Title
{
    public class TitleManager : SingletonMonoBehaviour<TitleManager>
    {
        [SerializeField] private SongPlayLoader _songPlayLoader;
        [SerializeField] private SceneTransition _sceneLoad;
        private Subject<SongSelectData> _onStartPlay = new();
        public Observable<SongSelectData> OnStartPlay  => _onStartPlay;

        public async void StartPlay(SongSelectData songSelectData)
        {
            var patterns = await CustomDataLoader.I.GetCustomPattern();
            var usePattern = patterns.Where(x => x.IsSelect).First();
            _songPlayLoader.CreatePlayManager(songSelectData, usePattern);
            _sceneLoad.ChangeScene("InGame");

            _onStartPlay.OnNext(songSelectData);
        }
    }

}