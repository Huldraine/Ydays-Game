using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HazardZone : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            // On passe par PlayerRespawn qui gère :
            // - dégâts via Health
            // - soft respawn (lastSafePosition) si encore vivant
            // - hard respawn (checkpoint) si mort
            respawn.OnHazardHit();
        }
    }
}
