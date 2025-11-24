using UnityEngine;

/// <summary>
/// Base générique pour les ennemis : vie, dégâts reçus, mort.
/// Ne gère pas le mouvement : utilise des scripts séparés (EnemyPatrol, etc.).
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

    [Header("Mort")]
    [Tooltip("Désactiver le GameObject au lieu de le détruire.")]
    public bool disableOnDeath = true;

    [Tooltip("Détruire l'ennemi au lieu de le désactiver.")]
    public bool destroyOnDeath = false;

    [Tooltip("Effet visuel optionnel instancié à la mort.")]
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

    /// <summary>
    /// Implémentation d'IDamageable. Appelée par HitBoxDamage, HazardZone, etc.
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
    /// Hook pour effets de dégâts (flash, son, animation, etc.).
    /// </summary>
    protected virtual void OnDamaged()
    {
        // Tu pourras ajouter plus tard un flash sprite, un hitstop, etc.
    }

    /// <summary>
    /// Gère la mort : stop physique, désactive colliders, VFX, etc.
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

        // Désactivation du collider pour éviter les collisions fantômes
        if (col != null)
        {
            col.enabled = false;
        }

        // VFX
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        // Destruction ou désactivation
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
