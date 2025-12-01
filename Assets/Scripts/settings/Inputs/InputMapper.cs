using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputMapper : MonoBehaviour
{
    // Last clicked button
    GameObject clickedButton;

    // Last key pressed
    KeyCode lasHitKey;

    // Flag to indicate if we are waiting for input
    bool isEditing = false;

    // Call InputManager script
    public InputManager inputManager;

    public void OnButtonClick()
    {
        // If we are already waiting for input, do nothing
        if (isEditing)
        {
            return;
        }

        // Get the current selected UI element
        EventSystem currentEvent = EventSystem.current;

        // Get the button that was clicked
        clickedButton = currentEvent.currentSelectedGameObject;
        
        // Change the button text to "?" to indicate waiting for input
        clickedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "...";
        
        // Set the editing flag to true
        isEditing = true;
    }

    void OnGUI()
    {
        // When we up the key, check if we are waiting for input
        if (Event.current.isKey && Event.current.type == EventType.KeyUp && isEditing)
        {
            // Save the key pressed
            lasHitKey = Event.current.keyCode;

            // If we are not editing, do nothing
            if (!isEditing)
            {
                return;
            }

            // We change the text of the button to the key pressed
            clickedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lasHitKey.ToString();
            // Update the key mapping in InputManager based on which button was clicked
            PlayerPrefs.SetString(clickedButton.name, lasHitKey.ToString());
            // We finish editing
            isEditing = false;

            // Update the key mappings in InputManager
            inputManager.UpdateMapping();
        }
    }
}
