using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;

    // List of object that we don't destroy on load
    public GameObject[] objects;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Dont destroy on load scene for each object in the list
        foreach (var element in objects)
        {
            DontDestroyOnLoad(element);
        }
    }
}
