using UnityEngine;

public class Interupteur : MonoBehaviour
{


    public bool isInRange = false;
    public bool interupteuractif = false;
    private Renderer rend;
    private bool isGreen = false;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            toggleInterupteur();
        }


    }

    void toggleInterupteur()
    {
        
        interupteuractif = true;
        
        // Changement de couleur
        if (isGreen)
            rend.material.color = Color.red;
        else
            rend.material.color = Color.green;

        isGreen = !isGreen;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
