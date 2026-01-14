using UnityEngine;

public class Language : MonoBehaviour
{
    [SerializeField] private string language;

    public static Language Instance;

    void Awake()
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
    }

    public void SetLanguage(string lang)
    {
        language = lang;
    }

    public string GetLanguage()
    {
        return language;
    }
}
