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
        if (LanguageManager.Instance != null)
        { 
            LanguageManager.Instance.OnLanguageChanged += UpdateText
        }
    }

    private void OnDisable()
    {

    }

    private void UpdateText()
    {

    }
}
