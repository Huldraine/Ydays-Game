using UnityEngine;

public class FlyingFollower : MonoBehaviour, IDamageable
{
    [Header("Suivi du joueur")]
    public Transform Player;        // le joueur à suivre
    public float speed = 2f;        // vitesse de déplacement
    public float maxDistance = 5f;  // distance à partir de laquelle il commence à suivre

    [Header("Vie")]
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // Ennemi volant : pas affecté par la gravité, pas poussé par les forces
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        if (Player == null) return;

        Vector2 direction = Player.position - transform.position;

        // Il commence à suivre seulement si le joueur est à moins de maxDistance
        if (direction.sqrMagnitude > maxDistance * maxDistance)
            return;

        // Mouvement simple dans l'air : on manipule directement la position
        Vector2 move = direction.normalized * speed * Time.deltaTime;
        transform.position += (Vector3)move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController2D pc = collision.gameObject.GetComponent<PlayerController2D>();
            if (pc != null)
            {
                // Knockback + dégâts + invincibilité côté joueur
                pc.OnHitByEnemy(transform.position, 1);
            }
        }
    }

    // Implémentation de IDamageable (HitBoxDamage, HazardZone peuvent l'utiliser)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
