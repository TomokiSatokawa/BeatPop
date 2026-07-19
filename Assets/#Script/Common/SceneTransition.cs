using System.Threading;
using Common.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    /// <summary>
    /// Scene移動
    /// </summary>
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private FadeImageControl _fadeImageControl;
        [Range(0.0f, 0.9f)]
        private float _loadCompleteProgress = 0.9f;
        private CancellationTokenSource _cancellation;
        private static bool _isLoading;
        public void ChangeScene(int sceneIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);

            ChangeSceneAsync(scenePath).Forget();
        }
        public void ChangeScene(string scenePath)
        {
            ChangeSceneAsync(scenePath).Forget();
        }

        private async UniTask ChangeSceneAsync(string scenePath)
        {
            if (_isLoading)
            {
                Debug.LogWarning("[SceneLoad] Sceneロード中に再要求されました");
                return;
            }

            _isLoading = true;

            try
            {
                UniTaskUtility.SafeDispose(ref _cancellation);
                _cancellation = new();

                if (_fadeImageControl != null)
                {
                    await _fadeImageControl.FadeOut(FadeType.White);
                }

                await LoadSceneAsync(scenePath, _cancellation.Token);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async UniTask LoadSceneAsync(string scenePath, CancellationToken token)
        {
            var op = SceneManager.LoadSceneAsync(scenePath);
            op.allowSceneActivation = false;

            if (op == null)
            {
                Debug.LogError($"[SceneLoad] Sceneロード失敗 : {scenePath}");
                return;
            }

            await UniTask.WaitUntil(() => op.progress >= _loadCompleteProgress, cancellationToken: token);

            op.allowSceneActivation = true;
            await op.ToUniTask(cancellationToken: token);
        }

        private void OnDestroy()
        {
            UniTaskUtility.SafeDispose(ref _cancellation);
        }
    }
}