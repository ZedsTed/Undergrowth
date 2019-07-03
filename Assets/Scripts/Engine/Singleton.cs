using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool quit = false;
    private static readonly object lockObj = new object();
    private static readonly Type type = typeof(T);

    protected static bool InstanceCheck()
    {
        if (instance == null)
            return false;

        // If we're a monobehaviour
        if (instance is MonoBehaviour)
        {
            MonoBehaviour monoBehaviour = instance as MonoBehaviour;

            // If we're null/destroyed, we don't really exist.
            if (monoBehaviour == null || monoBehaviour.transform == null)
                return false;
        }

        return true;
    }

    public static T Instance
    {
        get
        {
            lock (lockObj)
            {
                if (InstanceCheck())
                    return instance;

                if (quit)
                    return null;

                T[] instances = (T[])FindObjectsOfType(type);

                if (instances.Length > 0)
                    instance = instances[0];

                if (InstanceCheck())
                    return instance;

                GameObject singleton = new GameObject(type.Name);
                instance = singleton.AddComponent<T>();

                if (Application.isPlaying)
                    DontDestroyOnLoad(singleton);

                return instance;
            }
        }
    }

    /// <summary>
    /// Creates an instance of this singleton. Uses the get accessor of the Instance object to create it.
    /// </summary>
    /// <returns>Whether it was successful in creating it.</returns>
    public static bool CreateInstance()
    {
        return Instance != null;
    }

    /// <summary>
    /// Checks whether we have a valid instance of this singleton.
    /// </summary>
    public static bool HasInstance()
    {
        return instance != null;
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {          
            Debug.LogError(GetType().Name + ": Instance already exists. Destroying", gameObject);

            Destroy(this);

            return;
        }

        if (instance == null)
        {
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);

            instance = GetComponent<T>();
        }
    }

    public void OnDestroy()
    {     
        instance = null;
    }

    private void OnApplicationQuit()
    {
        quit = true;
    }
}
