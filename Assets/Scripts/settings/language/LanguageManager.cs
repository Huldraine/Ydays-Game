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
            return;
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
            { "quit", "Quit" },
            
            // Dictionnaire menu paramètres :
            { "settings_title", "SETTINGS" },
            { "screen", "Screen" },
            { "resolution", "Resolution" },
            { "fullscreen", "Fullscreen" },
            { "audio", "Sound" },
            { "volume", "Volume" },
            { "controls", "Controls" },
            { "move_up", "Move up" },
            { "move_down", "Move down" },
            { "move_left", "Move left" },
            { "move_right", "Move right" },
            { "credits", "Credits" }
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
            { "quit", "Quitter" },

            // Dictionnaire menu paramètres :
            { "settings_title", "PARAMÈTRES" },
            { "screen", "Écran" },
            { "resolution", "Résolution" },
            { "fullscreen", "Plein écran" },
            { "audio", "Son" },
            { "volume", "Volume" },
            { "controls", "Contrôles" },
            { "move_up", "Haut" },
            { "move_down", "Bas" },
            { "move_left", "Gauche" },
            { "move_right", "Droite" },
            { "credits", "Crédits" }
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

        // Prévenir tous les textes
        OnLanguageChanged?.Invoke();
    }

    public string getText(string key)
    {
        Dictionary<string, string> dict = (currentLanguage == Language.French) ? french : english;

        if (dict.TryGetValue(key, out string value))
            return value;

        // Si clé non trouvée --> renvoyer la clé pour debug
        return key;
    }

    public void SetFrench()
    {
        LanguageTranslate.Instance.setLanguage(Language.French);
    }

    public void SetEnglish()
    {
        LanguageTranslate.Instance.setLanguage(Language.English);
    }
}
