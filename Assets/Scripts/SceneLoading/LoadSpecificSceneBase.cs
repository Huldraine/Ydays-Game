using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class LoadSpecificSceneBase : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    protected abstract string sceneName { get; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
            return;

        if (!string.IsNullOrWhiteSpace(sceneName))
            SceneManager.LoadScene(sceneName);
    }
}