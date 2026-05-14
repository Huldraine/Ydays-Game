using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public GameObject PausePanel;
    private bool IsPaused = false;

    private static readonly string[] scenesWithoutPause = { "menu_principal", "parametres", "credits" };

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1f;
        if (PausePanel) PausePanel.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!PausePanel) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
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
        ForceResume();
        SceneManager.LoadScene("parametres");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu_principal");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ForceResume();

        bool hide = System.Array.IndexOf(scenesWithoutPause, scene.name) >= 0;
        gameObject.SetActive(!hide);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
