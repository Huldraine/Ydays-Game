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
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        initDictionaries();
        loadSavedLanguage();
    }
    private void initDictionaries()
    {
        english = new Dictionary<string, string>()
        {
            // Ajouter ici toutes les clés utilisées dans le jeu, avec leur traduction anglaise //
            // Exemple :
            // Clé = "pause" --> Valeur = "GAME PAUSED" //
            // Dictionnaire menu pause :
            { "pause", "GAME PAUSED" },
            { "resume", "Resume" },
            { "settings", "Settings" },
            { "save", "Save" },
            { "load", "Load" },
            { "exit", "Exit" },

            // Dictionnaire menu principal :
            { "play", "Play" },
            { "quit", "quit" }
        };

        french = new Dictionary<string, string>()
        {
            // Ajouter ici toutes les clés utilisées dans le jeu, avec leur traduction française //
            // Dictionnaire menu pause :
            { "pause", "MENU PAUSE" },
            { "resume", "Reprendre" },
            { "settings", "Paramètres" },
            { "save", "Sauvegarder" },
            { "load", "Charger" },
            { "exit", "Partir" },

            // Dictionnaire menu principal :
            { "play", "Jouer" },
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

    public void SetFrench()
    {
        LanguageTranslate.Instance.SetLanguage(Language.French);
    }

    public void SetEnglish()
    {
        LanguageTranslate.Instance.SetLanguage(Language.English);
    }
}
