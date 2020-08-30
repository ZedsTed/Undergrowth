using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class SingletonDontCreate<T> : SerializedMonoBehaviour
    where T : MonoBehaviour
{
    private static T instance;
    private static bool quit = false;
    private static readonly object lockObj = new object();
    private static readonly Type type = typeof(T);

    public static T Instance
    {
        get
        {
            if (quit)
                return null;

            lock (lockObj)
            {
                if (instance != null)
                    return instance;

                T[] instances = (T[])FindObjectsOfType(type);

                if (instances.Length > 0)
                    instance = instances[0];

                if (instance != null)
                    return instance;

                Debug.LogError(type.Name + ": Singleton instance does not exist");

                return null;
            }
        }
    }

    public static bool HasInstance()
    {
        return instance != null;
    }

    protected void Awake()
    {
        if (instance != null && instance != this as T)
        {
            Debug.LogError("Singleton<" + typeof(T).Name + ">.Awake: Instance is not null. Destroying.", gameObject);
            Destroy(this);
            return;
        }

        instance = this as T;

        SingletonInitialize();
    }

    protected void OnDestroy()
    {
        instance = null;

        SingletonTerminate();
    }

    protected virtual void OnApplicationQuit()
    {
        quit = true;
        instance = null;
    }


    protected virtual void SingletonInitialize() { }

    protected virtual void SingletonTerminate() { }
}
