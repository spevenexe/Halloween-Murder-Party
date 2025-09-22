using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public abstract class DontDestroySingleton<T> : Singleton<T> where T: Component
{
    protected override void Awake()
    {
        base.Awake();
        if (instance == this) DontDestroyOnLoad(gameObject);
    }
}