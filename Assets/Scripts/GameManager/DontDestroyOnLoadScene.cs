using UnityEngine;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    // List of object that we don't destroy on load
    public GameObject[] objects;

    void Awake()
    {
        foreach (var element in objects)
        {
            DontDestroyOnLoad(element);
        }
    }
}
