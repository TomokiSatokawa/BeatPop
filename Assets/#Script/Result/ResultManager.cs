using Common.PlaySystem;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private SceneLoad _sceneLoad;
    public void ReturnTitle()
    {
        ScoreManager.I.Release();
        JudgementManager.I.Release();
        SongPlayManager.I.Delete();

        _sceneLoad.ChangeScene("Title");
    }
}
