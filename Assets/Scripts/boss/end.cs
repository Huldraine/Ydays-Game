using UnityEngine;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private BossManger BossU;

    public bool isInRange = false;
    public bool interupteuractif = false;
    private Renderer rend;
    private bool isGreen = false;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (boss != null)
            BossU = boss.GetComponent<BossManger>();

        rend = GetComponent<Renderer>();
        if (rend != null && rend.material != null)
            rend.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            rend.material.color = Color.red;
            if (Input.GetKeyDown(KeyCode.E) && isInRange)
            {
                toggleInterupteur();
            }
        
            if (interupteuractif)
            {
                Debug.Log("end: interrupteur actif — chargement de la scène finale.");
                if (Application.CanStreamedLevelBeLoaded("EndScene"))
                {
                    SceneManager.LoadScene("EndScene");
                }
                else
                {
                    Debug.LogError("end: scène 'EndScene' introuvable dans les paramètres de build.");
                }
            }
        }
    }

    void toggleInterupteur()
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
