using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform startingCheckpoint; // checkpoint de départ (optionnel)

    [Header("Respawn après hazard (piques / vide)")]
    [Tooltip("Décalage appliqué par rapport à la dernière position safe quand on respawn depuis un hazard.")]
    [SerializeField] private Vector2 hazardRespawnOffset = new Vector2(-0.2f, 0.1f);
    // X négatif = léger retour en arrière, Y positif = un peu au-dessus de la plateforme

    // Pour plus tard : invulnérabilité & animations (à décommenter quand ce sera prêt)
    /*
    [Header("Invulnérabilité (à activer plus tard)")]
    [SerializeField] private float hazardInvulnerabilityTime = 1.0f; // durée d'invulnérabilité après un hazard
    [SerializeField] private Animator animator;                      // Animator du joueur
    [SerializeField] private string respawnTriggerName = "OnHazardRespawn";
    [SerializeField] private string invulnBoolName = "IsInvulnerable";

    private bool isInvulnerable;
    */

    private Rigidbody2D rb;

    private Vector3 checkpointPosition;  // dernier vrai checkpoint
    private Vector3 lastSafePosition;    // dernière position sûre (plateforme avant les piques)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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
    /// Appelé par une hazard (piques, vide) pour un respawn "léger".
    /// </summary>
    public void RespawnFromHazard()
    {
        // On respawn exactement à la dernière position safe calculée par le PlayerController
        TeleportTo(lastSafePosition);

        // (plus tard tu pourras lancer une anim ou une invulnérabilité ici)
    }



    /// <summary>
    /// Appelé quand le joueur meurt vraiment (PV = 0) → retour au checkpoint.
    /// Pour l'instant, tu peux l'appeler manuellement si besoin.
    /// </summary>
    public void RespawnFromDeath()
    {
        TeleportTo(checkpointPosition);
        lastSafePosition = checkpointPosition;

        // Plus tard, quand Health sera branché :
        // health.currentHealth = health.maxHealth;
        // health.TakeDamage(0f); // maj de l'UI des PV sans retirer de vie
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

    // ================================
    // Invulnérabilité & anim (optionnel)
    // ================================
    /*
    // ⚠️ Si tu décommente ça, pense à ajouter :
    // using System.Collections;
    // en haut du fichier.

    private IEnumerator HazardRespawnRoutine()
    {
        // 1) On téléporte d'abord le joueur à la position offsetée
        Vector3 targetPos = lastSafePosition + (Vector3)hazardRespawnOffset;
        TeleportTo(targetPos);

        // 2) On active l'invulnérabilité
        isInvulnerable = true;

        // 3) On lance une anim de respawn si un Animator est assigné
        if (animator != null && !string.IsNullOrEmpty(respawnTriggerName))
            animator.SetTrigger(respawnTriggerName);

        // 4) TODO : faire clignoter le sprite ici pendant hazardInvulnerabilityTime
        //    (ex: activer/désactiver SpriteRenderer.enabled toutes les 0.1s)

        // Exemple basique (à adapter) :
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // float t = 0f;
        // while (t < hazardInvulnerabilityTime)
        // {
        //     if (sr != null)
        //         sr.enabled = !sr.enabled;
        //     yield return new WaitForSeconds(0.1f);
        //     t += 0.1f;
        // }
        // if (sr != null) sr.enabled = true;

        // 5) On coupe l'état d'invulnérabilité sur l'Animator si besoin
        if (animator != null && !string.IsNullOrEmpty(invulnBoolName))
            animator.SetBool(invulnBoolName, false);

        isInvulnerable = false;
    }
    */
}
