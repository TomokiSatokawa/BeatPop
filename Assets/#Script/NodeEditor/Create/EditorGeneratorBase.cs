using InGame;
using UnityEngine;
public abstract class EditorGeneratorBase<T> : SingletonMonoBehaviour<T> where T:MonoBehaviour
{
    [SerializeField] private float _extraClone;

    protected virtual void Update()
    {
        double extraTime = _extraClone / EditorManager.I.Magnification;

        double displayTime =
            EditorManager.I.DisplayRange / EditorManager.I.Magnification;

        double minTime = StageTimeController.StageTime - extraTime;

        double maxTime = StageTimeController.StageTime + displayTime + extraTime;

        UpdateInRange(minTime, maxTime);
    }

    protected abstract void UpdateInRange(double minTime, double maxTime);
}