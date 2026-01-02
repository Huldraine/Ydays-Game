using UnityEngine;
using UnityEngine.UI;

public class interupteur : MonoBehaviour
{


    public bool isInRange = false;
    public bool interupteuractif = false;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            Interupteur();
        }


    }

    void Interupteur()
    {
        
        interupteuractif = true;


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
