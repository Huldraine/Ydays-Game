using UnityEngine;
using UnityEngine.SceneManagement;

public class close_settings : MonoBehaviour
{
    // Retour menu principal
    public void CloseSettings()
    {
        SceneManager.LoadScene(2);
    }
}
