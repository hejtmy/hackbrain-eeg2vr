using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    /**
       Returns the instance of this singleton.
    */
    public static T Get
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null) Debug.LogError("An instance of " + typeof(T) + 
                    " is needed in the scene, but there is none.");
            }
            return instance;
        }
    }

    //ADDED return new or existing instance of this singleton.
    public static T GetOrCreate (GameObject parent)
    {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    instance = parent.AddComponent<T>();
                }
            }
            return instance;
    }

    public static T GetOrCreate()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name);
                instance = go.AddComponent<T>();
            }
        }
        return instance;
    }
}