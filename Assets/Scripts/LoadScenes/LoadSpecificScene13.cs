using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScene13 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("niveau1/S10");
        }
    }
}
