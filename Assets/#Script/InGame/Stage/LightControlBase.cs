namespace InGame.Stage
{
    using UnityEngine;

    public abstract class LightControlBase : MonoBehaviour
    {
        public abstract void SetColor(Color color);
        public abstract void Flash(float duration, float power);
        public abstract void Wave(float duration, float power);
        public abstract void SetPower(float power);
        public abstract void Refresh();
    }
}