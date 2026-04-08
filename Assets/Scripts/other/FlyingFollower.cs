using UnityEngine;

public class FlyingFollower : EnemyBase
{
    [Header("Suivi du joueur")]
    public Transform Player;        // le joueur � suivre
    public float speed = 2f;        // vitesse de d�placement
    public float maxDistance = 5f;  // distance � partir de laquelle il commence � suivre

    protected override void Start()
    {
        base.Start();

        // Ennemi volant : pas affect� par la gravit�, pas pouss� par les forces
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

        // Il commence � suivre seulement si le joueur est � moins de maxDistance
        if (direction.sqrMagnitude > maxDistance * maxDistance)
            return;

        // Mouvement simple dans l'air : on manipule directement la position
        Vector2 move = direction.normalized * speed * Time.deltaTime;
        transform.position += (Vector3)move;
    }

}
