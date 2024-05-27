using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private static GameObject prefab;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private static void SetupInstance()
    {
        instance = (T)FindObjectOfType(typeof(T));
        if (instance == null)
        {
            GameObject gameObj = prefab;
            gameObj.name = typeof(T).Name;
            instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }

    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}