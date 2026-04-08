using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void creditsRedirect()
    {
        SceneManager.LoadScene("credits");
    }

    // Compatibility alias for existing button bindings.
    public void credits_redirect()
    {
        creditsRedirect();
    }
}
