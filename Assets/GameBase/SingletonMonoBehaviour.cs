using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T I;

    public virtual void Awake()
    {
        if (I == null)
        {
            I = this as T;
        }

        if (I != this)
        {
            Debug.LogError($"{typeof(T).Name} Ç™2Ç¬à»è„ë∂ç›ÇµÇ‹Ç∑");
            Destroy(this);
        }
    }
    protected virtual void OnDestroy()
    {
        if (I == this)
        {
            I = null;
        }
    }
}
