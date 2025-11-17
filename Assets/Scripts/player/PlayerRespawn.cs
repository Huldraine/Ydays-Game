using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform startingCheckpoint; // checkpoint de départ (optionnel)

    [Header("Dégâts des hazards (piques / vide)")]
    [SerializeField] private float hazardDamage = 1f; // quantité de vie perdue par chute/piques

    private Rigidbody2D rb;
    private Health health;

    private Vector3 checkpointPosition;  // dernier vrai checkpoint
    private Vector3 lastSafePosition;    // dernière position sûre (plateforme avant les piques)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();     // peut être null si pas encore mis, on gère le cas

        // Point de checkpoint de base
        if (startingCheckpoint != null)
            checkpointPosition = startingCheckpoint.position;
        else
            checkpointPosition = transform.position;

        // Au début, la position safe = checkpoint
        lastSafePosition = checkpointPosition;
    }

    /// <summary>
    /// Appelé par le PlayerController quand le joueur est bien au sol.
    /// </summary>
    public void UpdateLastSafePosition(Vector3 pos)
    {
        lastSafePosition = pos;
    }

    /// <summary>
    /// Appelé par une zone de type Hazard (piques, vide).
    /// Gère la perte de vie + choix entre soft/hard respawn.
    /// </summary>
    public void OnHazardHit()
    {
        if (health != null)
        {
            // on applique les dégâts
            health.TakeDamage(hazardDamage);

            if (health.currentHealth > 0)
            {
                // encore vivant → respawn proche
                RespawnFromHazard();
            }
            else
            {
                // mort → respawn au checkpoint + vie remise au max
                RespawnFromDeath();
            }
        }
        else
        {
            // pas de système de vie encore branché → juste respawn proche
            RespawnFromHazard();
        }
    }

    /// <summary>
    /// Respawn léger (après hazard) à la dernière position safe.
    /// </summary>
    public void RespawnFromHazard()
    {
        TeleportTo(lastSafePosition);
        // plus tard : invulnérabilité / anim ici
    }

    /// <summary>
    /// Respawn après une vraie mort → checkpoint + reset position safe.
    /// </summary>
    public void RespawnFromDeath()
    {
        // si Health est présent, on remet la vie au max + maj UI
        if (health != null)
        {
            health.currentHealth = health.maxHealth;
            health.TakeDamage(0f); // déclenche MettreAJourLesMasques sans retirer de PV
        }

        TeleportTo(checkpointPosition);
        lastSafePosition = checkpointPosition;
    }

    /// <summary>
    /// Appelé par un checkpoint (banc, statue…).
    /// </summary>
    public void SetCheckpoint(Transform checkpoint)
    {
        checkpointPosition = checkpoint.position;
        lastSafePosition = checkpointPosition;
    }

    private void TeleportTo(Vector3 pos)
    {
        transform.position = pos;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
