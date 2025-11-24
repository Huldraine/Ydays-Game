using UnityEngine;

public class ChangeColorEvery10Seconds : MonoBehaviour
{
    private Renderer rend;
    private bool isGreen = true;
    private float timer = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.green;   // Couleur initiale
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Toutes les 10 secondes, on change la couleur
        if (timer >= 10f)
        {
            if (isGreen)
                rend.material.color = Color.red;
            else
                rend.material.color = Color.green;

            isGreen = !isGreen;
            timer = 0f;
        }
    }
}
