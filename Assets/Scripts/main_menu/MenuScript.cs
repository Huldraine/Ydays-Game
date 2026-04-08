using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Jouer()
    {
        SceneManager.LoadScene("pnj_dialogues");
    }

    public void Parametres()
    {
        SceneManager.LoadScene("parametres");
    }

    public void Quitter()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
