using UnityEngine;

public abstract class SingletonPersistent<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
{
    public override void Awake()
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
    }

    public static void DisposeSingleton()
    {
        if (I != null)
        {
            Destroy(I.gameObject);
        }
    }
}