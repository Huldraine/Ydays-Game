using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text FPStext;
    public bool ShowFPS = true;
    private float timer;


    void Update()
    {
        if (!ShowFPS && FPStext == null)
            return;

        timer += Time.deltaTime;
        if (timer >= 0.5f) // Actualisation toutes les 0,5s
        {
            float FPS = 1f / Time.deltaTime;
            FPStext.text = $"{Mathf.RoundToInt(FPS)} FPS";

            if (FPS > 100)
                FPStext.color = Color.green;
            else if (FPS >= 30)
                FPStext.color = new Color(1f, 0.64f, 0f); // orange
            else
                FPStext.color = Color.red;

            timer = 0f;
        }
    }
}