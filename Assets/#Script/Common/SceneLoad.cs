using Common.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    [SerializeField] private FadeImageControl _fadeImageControl;
    public void ChangeScene(int sceneIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);

       ChangeScene(scenePath);
    }

    public async void ChangeScene(string scenePath)
    {
        await _fadeImageControl.FadeOut(FadeType.White);

        await LoadSceneAsync(scenePath);
    }

    private async UniTask LoadSceneAsync(string scenePath)
    {
        var op = SceneManager.LoadSceneAsync(scenePath);
        op.allowSceneActivation = false;

        await UniTask.WaitUntil(() => op.progress >= 0.9f);

        op.allowSceneActivation = true;
        await op.ToUniTask();
    }
}