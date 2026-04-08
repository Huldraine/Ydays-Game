using UnityEngine;

public class Follower : EnemyBase
{
    [Header("Suivi du joueur")]
    public Transform Player;        // le joueur à suivre
    public float speed = 2f;        // vitesse de déplacement horizontale
    public float maxDistance = 5f;  // distance d'aggro en X

    protected override void Start()
    {
        base.Start();

        // Ennemi au sol : soumis à la gravité, mais pas de rotation bizarre
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void FixedUpdate()
    {
        if (Player == null || rb == null) return;

        // On ne regarde que la distance horizontale
        float dx = Player.position.x - transform.position.x;
        float absDx = Mathf.Abs(dx);

        // L'ennemi ne bouge que si le joueur est à moins de maxDistance en X
        if (absDx > maxDistance)
            return;

        // Direction horizontale uniquement (-1 à gauche, +1 à droite)
        float directionX = Mathf.Sign(dx);

        // Si on est quasiment alignés, on ne bouge plus
        if (absDx < 0.05f)
            directionX = 0f;

        // Mouvement uniquement en X, on laisse la gravité gérer Y
        Vector2 move = new Vector2(directionX * speed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(rb.position + move);
    }

}