using UnityEngine;

/// <summary>
/// Inflige des dégâts au joueur au contact, avec un cooldown.
/// À mettre sur le même GameObject que l'ennemi (collider non-trigger).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Dégâts au joueur")]
    [Tooltip("Dégâts infligés au joueur au contact.")]
    public int contactDamage = 1;

    [Tooltip("Temps minimal entre deux applications de dégâts (en secondes).")]
    public float damageInterval = 0.5f;

    private float lastDamageTime = -999f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si jamais tu veux utiliser un collider en trigger pour les dégâts
        TryDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < lastDamageTime + damageInterval)
            return;

        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
            lastDamageTime = Time.time;
        }
    }
}
