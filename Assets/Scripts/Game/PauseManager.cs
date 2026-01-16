    using UnityEngine;

    public class PauseMenu : MonoBehaviour
    {
        public GameObject PausePanel;
        private bool IsPaused = false;

    void Start() {
        Resume();
            }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsPaused)
                { 
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }
    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Parameter()
    {
        
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}