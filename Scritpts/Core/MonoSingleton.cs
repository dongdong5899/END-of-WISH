using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T instance;

    static public T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null )
                {
                    Debug.LogError($"{typeof(T)}가 없어요. 스크립트를 넣어주세요.");
                }
            }
            return instance;
        }
    }
}
