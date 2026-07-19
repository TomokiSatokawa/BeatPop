using UnityEngine;
using Common;
using Cysharp.Threading.Tasks;
public class ResultManager : MonoBehaviour
{
    [SerializeField] private SceneTransition _sceneLoad;
    public void ReturnTitle()
    {
        GameManager.DontDestroyRelease();
        _sceneLoad.ChangeScene("Title");
    }
}
