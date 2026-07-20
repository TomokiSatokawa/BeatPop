using UnityEngine;

namespace InGame.Stage
{
    public abstract class StageLightBase : MonoBehaviour
    {
        public abstract void SetColor(Color color);
        public abstract void Flash(float duration, float power);
        public abstract void SetPower(float power);
        public abstract void Refresh();
    }
}