using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using InGame.Score;
using Sound;
using Title.SongSelect;
using UnityEngine;

namespace Result
{

    public class ResultSoundManager : MonoBehaviour
    {
        [SerializeField] private RankDataBase _rankData;
        [SerializeField] private float _bgmStartWaitTime;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _bgmVolume;
        private CancellationTokenSource _cancellation = new();

       private  async void Start()
        {
            await UniTask.WaitForSeconds(_bgmStartWaitTime, cancellationToken: _cancellation.Token);

            int score = ScoreDataManager.ScoreData.Score.CurrentValue;
            int maxScore = ScoreDataManager.ScoreData.MaxScore;
            float scoreRate = maxScore > 0 ? score / (float)maxScore : 0f;

            SoundManager.BGM.PlayBGM(_rankData.GetRank(scoreRate).Clip, _bgmVolume, isLoop: true);
        }

        public async UniTask FadeOut()
        {
            SoundManager.BGM.VolumeFade(0, _fadeDuration);
            await UniTask.WaitForSeconds(_fadeDuration,cancellationToken: _cancellation.Token);
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
            _cancellation = null;   
        }
    }
}