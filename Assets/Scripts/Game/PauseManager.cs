    using UnityEngine;

    public class PauseMenu : MonoBehaviour
    {
        public GameObject PausePanel;
        private bool IsPaused = false;

    void Start() {
        resume();
            }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsPaused)
                {
                pause();
                }
                else
                {
                resume();
                }
            }
        }
    public void resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    void pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void quitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
