using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    public string key;

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (textComponent == null) {
            textComponent = GetComponent<TMP_Text>();
        }

        if (LanguageTranslate.Instance != null)
        { 
            LanguageTranslate.Instance.OnLanguageChanged += updateText;
        }
        updateText();
    }

    private void OnDisable()
    {
        if (LanguageTranslate.Instance != null)
        { 
            LanguageTranslate.Instance.OnLanguageChanged -= updateText;
        }
    }

    private void updateText()
    {
        if (LanguageTranslate.Instance == null) return;

        string translated = LanguageTranslate.Instance.getText(key);
        textComponent.text = translated;
    }
}

