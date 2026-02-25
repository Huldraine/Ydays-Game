using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Key mappings for player movement
    public KeyCode upKeyCode;
    public KeyCode downKeyCode;
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;

    private void Start()
    {
        // Initialize key mappings
        UpdateMapping();
    }

    public void UpdateMapping()
    {
        // Load key mappings from PlayerPrefs

        // Default key is "W"
        string upKeyText = PlayerPrefs.GetString("UpAction", "W");
        upKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), upKeyText);
        
        // Default key is "S"
        string downKeyText = PlayerPrefs.GetString("DownAction", "S");
        downKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), downKeyText); 
        
        // Default key is "A"
        string leftKeyText = PlayerPrefs.GetString("LeftAction", "A");
        leftKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), leftKeyText);
        
        // Default key is "D"
        string rightKeyText = PlayerPrefs.GetString("RightAction", "D");
        rightKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), rightKeyText);
    }
}
