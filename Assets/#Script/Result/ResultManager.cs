using System.Threading;
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
        [SerializeField] private ResultSoundManager _soundManager;

        private void Start()
        {
            if (_sceneLoad == null)
                _sceneLoad = FindAnyObjectByType<SceneTransition>();
        }

        public async void ReturnTitle()
        {
            GameManager.DontDestroyRelease();

            UniTask fade = _fadeImageControl.FadeOut(FadeType.White);
            UniTask bgmFade = _soundManager.FadeOut();

            await UniTask.WhenAll(fade, bgmFade);
            await _sceneLoad.LoadSceneAsync("Title",new CancellationTokenSource().Token);
        }
    }
    public enum ResultType
    {
        Clear,FullCombo,AllPerfect
    }
}