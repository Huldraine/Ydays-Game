    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PauseMenu : MonoBehaviour
    {
        public GameObject PausePanel;
        private bool IsPaused = false;

    void Start() { PausePanel.SetActive(IsPaused); }
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
            // Protection panel non bind
            if (!PausePanel) return; 
            
            IsPaused = !IsPaused;
            
            Time.timeScale = IsPaused ? 0f : 1f;
            PausePanel.SetActive(IsPaused);
        }

    public void Settings()
    {
        SceneManager.LoadScene("pnj_dialogues pierre"); // Paramètre pierre
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void RebindPausePanel(Scene scene, LoadSceneMode mode)
    {
        // Effacer l'ancienne référence détruite au chargement de la sauvegarde
        PausePanel = null;

        // Rechercher le nouveau panel pour le réassigner
        GameObject found = GameObject.FindWithTag("PauseUI");
        if (found != null)
        {
            PausePanel = found;
            PausePanel.SetActive(false);
        }
        
        // Forcer la reprise du jeu
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