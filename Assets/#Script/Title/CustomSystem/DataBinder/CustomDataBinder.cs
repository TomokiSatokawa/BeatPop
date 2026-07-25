using UnityEngine;

namespace Title.Custom
{
    public abstract class CustomDataBinder<T> : MonoBehaviour
    {
        public abstract void SetCustom(T data);
        public abstract T GetCustom();
        public abstract void OnDefault();
    }
}