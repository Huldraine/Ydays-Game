using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HazardZone : MonoBehaviour
{
    [Header("Général")]
    [Tooltip("Est-ce que cette zone doit affecter le joueur ?")]
    public bool affectPlayer = true;

    [Tooltip("Est-ce que cette zone doit affecter les autres objets qui peuvent prendre des dégâts (ennemis, etc.) ?")]
    public bool affectOtherDamageables = true;

    [Tooltip("Dégâts infligés aux objets IDamageable (ennemis, etc.). Pour un kill instant, mets une valeur très haute.")]
    public int damageToOthers = 999;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) Cas spécial : le joueur (géré par PlayerRespawn)
        if (affectPlayer)
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                // Le PlayerRespawn s'occupe :
                // - d'appliquer les dégâts au joueur via Health
                // - de choisir soft respawn / hard respawn
                respawn.OnHazardHit();
                return; // IMPORTANT : on ne continue pas, pour éviter de double-dégâts via IDamageable
            }
        }

        // 2) Tous les autres damageables (ennemis etc.)
        if (affectOtherDamageables)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageToOthers);
            }
        }
    }
}
