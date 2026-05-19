using UnityEngine;

public class WpSingle : MonoBehaviour, IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public float currentHealth;

    [Header("wind")]
    [SerializeField] private GameObject wind;

    private Renderer rend;

    [SerializeField] public bool windActive;

    public Rigidbody2D rb;
    public Collider2D col;
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        currentHealth = 90f;
        windActive = false;
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;   // Couleur initiale
    }

    public void Update()
    {
        if (getLife()<= 0)
        {
            Destroy(gameObject);
        }
        
        if (windActive == true)
        {
            rend.material.color = Color.green;   // Couleur si fonctionnel
        } else
        {
            rend.material.color = Color.red;   // Couleur si non fonctionnel
        }
    }

    public float getLife()
    {
        return currentHealth;
    }

    public void setLife(float newLife)
    {
        currentHealth = newLife;
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
