using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// エディターのデータ管理ベースクラス
    /// </summary>
    public abstract class EditorDataManagerBase<T> : SingletonMonoBehaviour<T> where T : EditorDataManagerBase<T>
    {
        [SerializeField] private Button _exportButton;
        [SerializeField] private Button _importButton;
        [SerializeField] private Button _newFileButton;

        protected const double Epsilon = 0.0001;

        private void Start()
        {
            if (_exportButton != null)
                _exportButton.onClick.AddListener(() => EditorFilePicker.OnExport(GetSerializeJson()));

            if (_importButton != null)
                _importButton.onClick.AddListener(() => EditorFilePicker.OnImport(DeserializeJson));

            if (_newFileButton != null)
                _newFileButton.onClick.AddListener(CreateNewFile);
        }

        protected abstract string GetSerializeJson();
        protected abstract void DeserializeJson(string json);
        protected abstract void CreateNewFile();
    }
}