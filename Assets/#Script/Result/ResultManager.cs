using Common;
using UnityEngine;

namespace Result.UI
{
    /// <summary>
    /// リザルトシールの管理クラス
    /// </summary>
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private SceneTransition _sceneLoad;

        private void Start()
        {
            if(_sceneLoad == null)
                _sceneLoad = FindAnyObjectByType<SceneTransition>();
        }

        public void ReturnTitle()
        {
            GameManager.DontDestroyRelease();
            _sceneLoad.ChangeScene("Title");
        }
    }
}