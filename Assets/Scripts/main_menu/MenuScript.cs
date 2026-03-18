using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Jouer()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Parametres()
    {
        SceneManager.LoadScene("parametres");
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
