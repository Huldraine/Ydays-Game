using UnityEngine;

public class HitBoxDamage : MonoBehaviour
{
    [Header("Dégâts")]
    public int damage = 1;

    [Header("Pogo (optionnel)")]
    [Tooltip("Activer uniquement sur la hitbox bas du joueur.")]
    public bool causePogo = false;      // coche ça sur la hitboxDown
    public float pogoForce = 10f;       // force de base du pogo

    private PlayerController2D playerController;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        // On suppose que la hitbox est un enfant du Player
        playerController = GetComponentInParent<PlayerController2D>();
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si cette hitbox appartient au joueur, on ne fait rien
        // si le joueur n'est pas en plein dans une attaque
        if (playerAttack != null && !playerAttack.IsAttacking)
            return;

        bool hitSomething = false;

        // 1) Tout ce qui peut prendre des dégâts (ennemi, joueur, autre, plus tard)
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            hitSomething = true;
        }

        // 2) Surfaces de pogo (piques, blocs spéciaux, etc.)
        PogoSurface pogoSurface = other.GetComponent<PogoSurface>();
        if (pogoSurface != null)
        {
            hitSomething = true;
        }

        // 3) Pogo ?
        if (causePogo && hitSomething && playerController != null && playerAttack != null)
        {
            // On exige une vraie attaque vers le bas
            if (!playerAttack.IsDownAttackActive)
                return;

            // Et que le joueur soit en train de tomber
            if (playerController.VerticalVelocity < -0.01f)
            {
                float finalPogoForce = pogoForce;

                if (pogoSurface != null)
                    finalPogoForce = pogoSurface.ComputePogoForce(finalPogoForce);

                playerController.ApplyPogo(finalPogoForce);
            }
        }
    }
}
