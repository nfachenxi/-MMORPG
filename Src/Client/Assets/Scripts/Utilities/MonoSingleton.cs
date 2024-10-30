using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    void Start()
    {
        if(instance != null && instance != this.gameObject.GetComponent<T>())
        {
            Destroy(this.gameObject);
            return;
        }
        if (global) DontDestroyOnLoad(this.gameObject);
        this.OnStart();
        instance = this.gameObject.GetComponent<T>();
    }

    protected virtual void OnStart()
    {

    }
}