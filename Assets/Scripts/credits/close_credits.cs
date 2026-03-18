using UnityEngine;
using UnityEngine.SceneManagement;

public class close_credits : MonoBehaviour
{
    // Retour aux paramètres
    public void Close_Credits()
    {
        Debug.Log("CLIC DETECTE");
        SceneManager.LoadScene("parametres");
    }
}
