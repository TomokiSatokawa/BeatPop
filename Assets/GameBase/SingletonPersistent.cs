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
            DisposeSingleton();
            return;
        }

        this.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public void DisposeSingleton()
    {
        Destroy(this.gameObject);
    }
}