using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool IsPaused = false;

    void Start()
    {
        PausePanel.SetActive(IsPaused);
    }

    void Update()
    {
        if (!PausePanel) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ForceResume()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        if (PausePanel)
            PausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (!PausePanel) return;

        IsPaused = !IsPaused;

        Time.timeScale = IsPaused ? 0f : 1f;
        PausePanel.SetActive(IsPaused);
    }

    public void Parameter()
    {
        SceneManager.LoadScene("parametres");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            SceneManager.LoadScene("main_menu");
        #endif
    }

    private void RebindPausePanel(Scene scene, LoadSceneMode mode)
    {
        PausePanel = null;

        GameObject pauseui = GameObject.FindWithTag("PauseUI");
        if (pauseui != null)
        {
            PausePanel = pauseui;
            PausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
        IsPaused = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += RebindPausePanel;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= RebindPausePanel;
    }
}
