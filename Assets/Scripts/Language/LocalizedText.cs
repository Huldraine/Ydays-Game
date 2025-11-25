using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    public string key;

    private TMP_Text textComponent;

    public void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (LanguageTranslate.Instance != null)
        { 
            LanguageTranslate.Instance.OnLanguageChanged += UpdateText;
        }
        UpdateText();
    }

    private void OnDisable()
    {
        if (LanguageTranslate.Instance != null)
        { 
            LanguageTranslate.Instance.OnLanguageChanged -= UpdateText;
        }
    }

    private void UpdateText()
    {
        if (LanguageTranslate.Instance == null) return;

        string translated = LanguageTranslate.Instance.GetText(key);
        textComponent.text = translated;
    }
}
