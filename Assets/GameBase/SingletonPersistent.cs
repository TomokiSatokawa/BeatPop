using UnityEngine;

public abstract class SingletonPersistent<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
{
    public sealed override void Awake()
    {
        if (I == null)
        {
            I = this as T;
        }

        if (I != this)
        {
            Destroy(this.gameObject);
            return;
        }

        this.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public static void DisposeSingleton()
    {
        if (I != null)
        {
            Destroy(I.gameObject);
        }
    }
}