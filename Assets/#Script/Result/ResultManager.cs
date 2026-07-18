using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private SceneLoad _sceneLoad;
    public void ReturnTitle()
    {
        GameManager.DontDestroyRelease();
        _sceneLoad.ChangeScene("Title");
    }
}
