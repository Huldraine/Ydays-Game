using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Jouer()
    {
        SceneManager.LoadScene(0);
    }

    public void Parametres()
    {
        SceneManager.LoadScene(1);
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
