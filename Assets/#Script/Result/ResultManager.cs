using Common.PlaySystem;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private SceneLoad _sceneLoad;
    public void ReturnTitle()
    {
        ScoreManager.I.Delete();
        JudgementManager.I.Delete();
        SongPlayManager.I.Delete();

        _sceneLoad.ChangeScene("Title");
    }
}
