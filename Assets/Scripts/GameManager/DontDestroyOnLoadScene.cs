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
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        foreach (var element in objects)
        {
            if (element == null) continue;

            element.transform.SetParent(null);
            DontDestroyOnLoad(element);
        }
    }
}
