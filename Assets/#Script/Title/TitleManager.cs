using System.Linq;
using Common.PlaySystem;
using R3;
using UnityEngine;
namespace Title
{
    public class TitleManager : SingletonMonoBehaviour<TitleManager>
    {
        [SerializeField] private SongPlayLoader _songPlayLoader;
        [SerializeField] private SceneLoad _sceneLoad;
        private Subject<SongSelectData> _onStartPlay = new();
        public Observable<SongSelectData> OnStartPlay  => _onStartPlay;

        public async void StartPlay(SongSelectData songSelectData)
        {
            var patterns = await CustomDataLoader.I.GetCustomPattern(songSelectData.SongData.SongID);
            var usePattern = patterns.Where(x => x.IsSelect).First();
            _songPlayLoader.OnLoad(songSelectData, usePattern);
            _sceneLoad.ChangeScene("InGame");

            _onStartPlay.OnNext(songSelectData);
        }
    }

}