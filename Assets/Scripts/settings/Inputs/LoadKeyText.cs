using UnityEngine;
using TMPro;

public class LoadKeyText : MonoBehaviour
{
    TMP_Text text;

    void Start()
    {
        // Try to get the TMP_Text component on this GameObject first
        text = GetComponent<TMP_Text>();
        
        // If not found, try to get it from the first child
        if (text == null)
        {
            text = GetComponentInChildren<TMP_Text>();
        }
        
        if (text == null)
        {
            Debug.LogError($"No TMP_Text component found on {gameObject.name} or its children!");
            return;
        }

        // Use this GameObject's name as the key (it should be the button name like "UpAction")
        string buttonName = gameObject.name;
        string savedKey = PlayerPrefs.GetString(buttonName, text.text);
        
        Debug.Log($"Loading key for button: {buttonName}, saved value: {savedKey}, default: {text.text}");
        
        text.text = savedKey;
    }
}
