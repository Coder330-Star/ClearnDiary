using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance = null)
            {
                Debug.Log(typeof(T).ToString() + "is null.");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this as T;
    }
}
