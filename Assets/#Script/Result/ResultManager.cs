using System.Threading;
using Common;
using Common.UI;
using Cysharp.Threading.Tasks;
using InGame.Score;
using Title.PlayerData;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// リザルトシーンの管理クラス
    /// </summary>
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private SceneTransition _sceneLoad;
        [SerializeField] private FadeImageControl _fadeImageControl;
        [SerializeField] private ResultSoundManager _soundManager;
        [SerializeField] private LevelData _levelData;

        private static PlayerInfo _previousPlayerInfo;
        public static IReadOnlyPlayerInfo PreviousPlayerInfo => _previousPlayerInfo;

        private void Start()
        {
            if (_sceneLoad == null)
                _sceneLoad = FindAnyObjectByType<SceneTransition>();

            _previousPlayerInfo = PlayerDataLoader.Info.Clone();

            var addXp = ScoreDataManager.ScoreData.GetXP();
            _levelData.LevelUp(_previousPlayerInfo.Level,_previousPlayerInfo.XP,addXp,out var endLevel,out var endXp);

            PlayerDataLoader.Info.UpdateLevelXp(endLevel,endXp);
        }

        public async void ReturnTitle()
        {
            GameManager.DontDestroyRelease();

            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);
            UniTask bgmFade = _soundManager.FadeOut();

            await UniTask.WhenAll(fade, bgmFade);
            await _sceneLoad.LoadSceneAsync("Title", new CancellationTokenSource().Token);
        }
    }
    public enum ResultType
    {
        Clear, FullCombo, AllPerfect
    }
}