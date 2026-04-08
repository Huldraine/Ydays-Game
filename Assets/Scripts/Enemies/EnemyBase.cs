using UnityEngine;

/// <summary>
/// Base g�n�rique pour les ennemis : vie, d�g�ts re�us, mort.
/// Ne g�re pas le mouvement : utilise des scripts s�par�s (EnemyPatrol, etc.).
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Vie")]
    [Tooltip("Points de vie maximum de l'ennemi.")]
    public int maxHealth = 3;

    [Tooltip("Points de vie actuels (debug).")]
    [SerializeField] private int currentHealth;

    [Header("Contact")]
    [Tooltip("Infliger des dégâts au joueur au contact.")]
    public bool damagePlayerOnContact = true;

    [Tooltip("Dégâts infligés au joueur au contact.")]
    public int contactDamage = 1;

    [Tooltip("Temps minimal entre deux dégâts au joueur.")]
    public float contactDamageInterval = 0.5f;

    private float lastContactDamageTime = -999f;

    [Header("Mort")]
    [Tooltip("D�sactiver le GameObject au lieu de le d�truire.")]
    public bool disableOnDeath = true;

    [Tooltip("D�truire l'ennemi au lieu de le d�sactiver.")]
    public bool destroyOnDeath = false;

    [Tooltip("Effet visuel optionnel instanci� � la mort.")]
    public GameObject deathVFX;

    protected Rigidbody2D rb;
    protected Collider2D col;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        tryDamagePlayer(collision.collider);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        tryDamagePlayer(collision.collider);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        tryDamagePlayer(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        tryDamagePlayer(other);
    }

    protected void tryDamagePlayer(Collider2D other)
    {
        if (!damagePlayerOnContact || other == null)
            return;

        if (Time.time < lastContactDamageTime + contactDamageInterval)
            return;

        PlayerController2D playerController = other.GetComponentInParent<PlayerController2D>();
        if (playerController != null)
        {
            playerController.OnHitByEnemy(transform.position, contactDamage);
            lastContactDamageTime = Time.time;
            return;
        }

        Health playerHealth = other.GetComponentInParent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
            lastContactDamageTime = Time.time;
        }
    }

    /// <summary>
    /// Impl�mentation d'IDamageable. Appel�e par HitBoxDamage, HazardZone, etc.
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        OnDamaged();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Hook pour effets de d�g�ts (flash, son, animation, etc.).
    /// </summary>
    protected virtual void OnDamaged()
    {
        // Tu pourras ajouter plus tard un flash sprite, un hitstop, etc.
    }

    /// <summary>
    /// G�re la mort : stop physique, d�sactive colliders, VFX, etc.
    /// </summary>
    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop physique
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        // D�sactivation du collider pour �viter les collisions fant�mes
        if (col != null)
        {
            col.enabled = false;
        }

        // VFX
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        // Destruction ou d�sactivation
        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else if (disableOnDeath)
        {
            gameObject.SetActive(false);
        }
    }
}
