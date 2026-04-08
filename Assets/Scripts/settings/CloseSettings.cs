using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseSettings : MonoBehaviour
{
    // Retour menu principal
    public void closeSettings()
    {
        SceneManager.LoadScene("menu_principal");
    }

    // Compatibility alias for existing button bindings.
    public void CloseSettingsButton()
    {
        closeSettings();
    }
}
