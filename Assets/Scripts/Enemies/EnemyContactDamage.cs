using UnityEngine;

/// <summary>
/// Inflige des d�g�ts au joueur au contact, avec un cooldown.
/// � mettre sur le m�me GameObject que l'ennemi (collider non-trigger).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyContactDamage : MonoBehaviour
{
    [Header("D�g�ts au joueur")]
    [Tooltip("D�g�ts inflig�s au joueur au contact.")]
    public int contactDamage = 1;

    [Tooltip("Temps minimal entre deux applications de d�g�ts (en secondes).")]
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
        // Si jamais tu veux utiliser un collider en trigger pour les d�g�ts
        TryDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider2D other)
    {
        if (Time.time < lastDamageTime + damageInterval)
            return;

        PlayerController2D playerController = other.GetComponentInParent<PlayerController2D>();
        if (playerController != null)
        {
            playerController.OnHitByEnemy(transform.position, contactDamage);
            lastDamageTime = Time.time;
            return;
        }

        Health playerHealth = other.GetComponentInParent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
            lastDamageTime = Time.time;
        }
    }
}
