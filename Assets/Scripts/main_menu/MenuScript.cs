using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void jouer()
    {
        SceneManager.LoadScene("poumonS1");
    }

    public void parametres()
    {
        SceneManager.LoadScene("parametres");
    }

    public void quitter()
    {
        Application.Quit();
    }
}

