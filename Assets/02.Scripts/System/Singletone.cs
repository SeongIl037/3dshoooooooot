using UnityEngine;

public class Singletone<T> : MonoBehaviour
{
    public static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
