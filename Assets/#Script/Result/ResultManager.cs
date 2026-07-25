using System.Threading.Tasks;
using Common;
using Common.UI;
using Cysharp.Threading.Tasks;
using InGame.Score;
using Sound;
using Title.SongSelect;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Result.UI
{
    /// <summary>
    /// リザルトシールの管理クラス
    /// </summary>
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private SceneTransition _sceneLoad;
        [SerializeField] private FadeImageControl _fadeImageControl;
        [SerializeField] private RankDataBase _rankData;
        [SerializeField] private float _fadeDuration;

        private void Start()
        {
            if (_sceneLoad == null)
                _sceneLoad = FindAnyObjectByType<SceneTransition>();

            int score = ScoreDataManager.ScoreData.Score.CurrentValue;
            int maxScore = ScoreDataManager.ScoreData.MaxScore;
            float scoreRate = maxScore > 0 ? score / (float)maxScore : 0f;

            SoundManager.BGM.PlayBGM(_rankData.GetRank(scoreRate).Clip);
        }

        public async Task ReturnTitle()
        {
            GameManager.DontDestroyRelease();

            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);
            UniTask bgmFade = UniTask.WaitForSeconds(_fadeDuration);

            SoundManager.BGM.VolumeFade(0,_fadeDuration);
            await UniTask.WhenAll(fade, bgmFade);
            _sceneLoad.ChangeScene("Title");
        }
    }
}