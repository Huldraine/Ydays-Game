using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CheckpointAuto : MonoBehaviour
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
            respawn.SetCheckpoint(transform);
            // ici plus tard tu pourras ajouter un son / anim
        }
    }
}
