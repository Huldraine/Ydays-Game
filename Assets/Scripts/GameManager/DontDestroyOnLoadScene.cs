using UnityEngine;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    // Singleton instance
    public static DontDestroyOnLoadScene Instance;

    // List of object that we don't destroy on load
    public GameObject[] objects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var element in objects)
        {
            DontDestroyOnLoad(element);
        }
    }
}
