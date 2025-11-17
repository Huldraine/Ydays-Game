using UnityEngine;

/// <summary>
/// Surface spéciale sur laquelle le joueur peut pogo via sa hitbox bas.
/// À mettre par exemple sur des piques.
/// Permet de modifier la force du pogo pour cette surface.
/// </summary>
public class PogoSurface : MonoBehaviour
{
    [Tooltip("Si >= 0, remplace directement la force de base du pogo.")]
    public float overridePogoForce = -1f;

    [Tooltip("Multiplicateur appliqué à la force de base si overridePogoForce < 0.")]
    public float pogoMultiplier = 1f;

    /// <summary>
    /// Calcule la force finale de pogo à partir de la force de base de la hitbox.
    /// </summary>
    public float ComputePogoForce(float baseForce)
    {
        if (overridePogoForce >= 0f)
        {
            return overridePogoForce;
        }

        return baseForce * pogoMultiplier;
    }
}
