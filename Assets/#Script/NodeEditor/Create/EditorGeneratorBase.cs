using InGame;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// エディターでの描画ベースクラス
    /// </summary>
    public abstract class EditorGeneratorBase<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        [SerializeField] private float _extraClone;

        private void Update()
        {
            double extraTime = _extraClone / EditorManager.I.Magnification;

            double displayTime =
                EditorManager.I.DisplayRange / EditorManager.I.Magnification;

            double minTime = StageTimeController.StageTime - extraTime;

            double maxTime = StageTimeController.StageTime + displayTime + extraTime;

            UpdateInRange(minTime, maxTime);
            OnUpdate();
        }
        protected abstract void UpdateInRange(double minTime, double maxTime);
        protected virtual void OnUpdate() { }
    }
}