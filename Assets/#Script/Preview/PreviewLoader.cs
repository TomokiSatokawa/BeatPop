using Cysharp.Threading.Tasks;
using Title;
using Title.Custom;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Preview
{
    /// <summary>
    /// プレビュー画面を表示非表示する
    /// </summary>
    public class PreviewLoader : MonoBehaviour
    {
        [SerializeField] private PatternUIList _patternUIList;
        [SerializeField] private ValueStepper _nodeSpeed;
        [SerializeField] private string _previewScene;
        [SerializeField] private GameObject _previewImage;

        private void Start()
        {
            _previewImage.SetActive(false);
        }

        public async void Load()
        {
            await SceneManager.LoadSceneAsync(_previewScene, LoadSceneMode.Additive);
            _previewImage.gameObject.SetActive(true);
            await UniTask.WaitForSeconds(1f);
            PreviewManager.I.OnChangeValue(_patternUIList.CurrentSelectData.SpeedPattern.NodeSpeed);
        }

        public void OnValueChange(float value)
        {
            if(PreviewManager.I != null)
                PreviewManager.I.OnChangeValue(ValueStepper.GetInterpolationFactor(_nodeSpeed, value));
        }

        public void Unload()
        {
            _previewImage.gameObject.SetActive(false);
            PreviewManager.DontDestroyRelease();
            SceneManager.UnloadSceneAsync(_previewScene);
        }
    }
}