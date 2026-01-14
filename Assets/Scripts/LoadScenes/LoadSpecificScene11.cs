using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScen11 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("niveau1/Z1");
        }
    }
}
