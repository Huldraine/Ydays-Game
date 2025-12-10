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
    private Dictionary<string, Language> langCodes; 

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

        InitDictionaries();
        InitLanguageCodes(); 
        LoadSavedLanguage();
    }

    private void InitLanguageCodes()
    {
        langCodes = new Dictionary<string, Language>()
        {
            { "en", Language.English },
            { "fr", Language.French }
        };    
    }

    private void InitDictionaries()
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
            { "settings", "Parametres" },
            { "save", "Sauvegarder" },
            { "quit", "Quitter" }
        };
    }
    private void LoadSavedLanguage()
    {
        string savedLang = PlayerPrefs.GetString("language", "en");
        if (langCodes.TryGetValue(savedLang, out Language lang))
            currentLanguage = lang;
        else
            currentLanguage = Language.English;
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;

        // Sauvegarder
        string code = (language == Language.French) ? "fr" : "en";
        PlayerPrefs.SetString("language", code);
        PlayerPrefs.Save();

        // Prévenir tous les textes
        OnLanguageChanged?.Invoke();
    }

    public string GetText(string key)
    {
        Dictionary<string, string> dict = (currentLanguage == Language.French) ? french : english;

        if (dict.TryGetValue(key, out string value))
            return value;

        // Si clé non trouvée --> renvoyer la clé pour debug
        return key;
    }
}
