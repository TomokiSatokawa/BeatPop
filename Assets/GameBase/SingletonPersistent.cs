using UnityEngine;

public abstract class SingletonPersistent<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
{
    public sealed override void Awake()
    {
        if (I == null)
        {
            Debug.Log("this");
            I = this as T;
        }

        if (I != this)
        {
            Debug.Log("des");
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
            I = null;
        }
    }
}