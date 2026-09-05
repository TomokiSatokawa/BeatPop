using System.Threading;
using Common;
using Common.PlaySystem;
using Common.UI;
using Cysharp.Threading.Tasks;
using InGame;
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
        [SerializeField] private ResultRewardData _rewardData;

        private static PlayerInfo _previousPlayerInfo;
        public static IReadOnlyPlayerInfo PreviousPlayerInfo => _previousPlayerInfo;

        private void Start()
        {
            if (_sceneLoad == null)
                _sceneLoad = FindAnyObjectByType<SceneTransition>();

            _previousPlayerInfo = PlayerDataLoader.Info.Clone();

            LevelUp();
            AddCoin();
            AddJewelry();
        }

        private void LevelUp()
        {
            var addXp = ScoreDataManager.ScoreData.GetXP();
            _levelData.LevelUp(_previousPlayerInfo.Level, _previousPlayerInfo.XP, addXp, out var endLevel, out var endXp);

            PlayerDataLoader.Info.UpdateLevelXp(endLevel, endXp);
        }

        private void AddCoin()
        {
            var difficulty = SongPlayContext.I.SongData.Difficulty;
            var resultType = ScoreDataManager.ScoreData.GetResultType();
            var rankType = StageConfig.I.RankDataBase.GetRank(ScoreDataManager.ScoreData.ScoreRate).RankType;

            int coinCount = _rewardData.GetCoinCount(difficulty, rankType, resultType);
            PlayerDataLoader.Info.AddCoin(coinCount);
        }

        private void AddJewelry()
        {
            int jewelryCount = _rewardData.GetJewelryCount(_previousPlayerInfo.Level, PlayerDataLoader.Info.Level);
            PlayerDataLoader.Info.AddJewelry(jewelryCount);
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