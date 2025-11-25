using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool IsPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0f;
                IsPaused = true;
            } else 
            {
                PausePanel.SetActive(false);
                Time.timeScale = 1f;
                IsPaused = false;
            }
        }
    }
}
