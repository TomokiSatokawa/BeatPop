namespace InGame.Stage
{
    using UnityEngine;

    public abstract class LightControlBase : MonoBehaviour
    {
        public abstract void SetColor(Color color);
        public abstract void Flash(float duration, float power);
    }
}