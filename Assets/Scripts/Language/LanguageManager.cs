using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English,
    French
}
public class LanguageTranslate : MonoBehaviour
{

    public static LanguageTranslate Instance { get; private set; }

    public Language currentLanguage = Language.English;

    // Dictionnaire de traductions // 
    private Dictionary<string, string> english;
    private Dictionary<string, string> french;

    //  //
    public delegate void LanguageChanged();
    public event LanguageChanged OnLanguageChanged;

    private void Awake()
    {
        // Singleton basique (= ) // 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        initDictionaries();
        loadSavedLanguage();
    }
    private void initDictionaries()
    {
        english = new Dictionary<string, string>()
        {
            { "pause", "GAME PAUSED" },
            { "resume", "Resume" },
            { "settings", "Settings" },
            { "save", "Save" },
            { "quit", "Quit" }
        };

        french = new Dictionary<string, string>()
        {
            { "pause", "MENU PAUSE" },
            { "resume", "Reprendre" },
            { "settings", "Parametre" },
            { "save", "Sauvegarder" },
            { "quit", "Quitter" }
        };
    }
    private void loadSavedLanguage()
    {
        string savedLang = PlayerPrefs.GetString("language", "en");
        if (savedLang == "fr")
            currentLanguage = Language.French;
        else
            currentLanguage = Language.English;
    }

    public void setLanguage(Language language)
    {
        currentLanguage = language;

        // Sauvegarder
        string code = (language == Language.French) ? "fr" : "en";
        PlayerPrefs.SetString("language", code);
        PlayerPrefs.Save();

        // PrÃ©venir tous les textes
        OnLanguageChanged?.Invoke();
    }

    public string getText(string key)
    {
        Dictionary<string, string> dict = (currentLanguage == Language.French) ? french : english;

        if (dict.TryGetValue(key, out string value))
            return value;

        // Si clÃ© non trouvÃ©e --> renvoyer la clÃ© pour debug
        return key;
    }

    public void setLanguageFromButton(string langCode)
    {
        if (langCode == "fr")
            setLanguage(Language.French);
        else
            setLanguage(Language.English);
    }
}

