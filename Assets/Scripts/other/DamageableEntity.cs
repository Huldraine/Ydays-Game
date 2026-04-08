using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class that wraps a <see cref="Health"/> component and exposes
/// the IDamageable interface. It also fires a callback when the
/// associated health reaches zero.
/// Derived classes can override <see cref="onDeathHandler"/> to react to
/// death events (disable, play VFX, etc.).
/// </summary>
[RequireComponent(typeof(Health))]
public abstract class DamageableEntity : MonoBehaviour, IDamageable
{
    protected Health health;

    /// <summary>Invoked by <see cref="Health"/> when health falls to 0.</summary>
    public UnityEvent onDeath = new UnityEvent();

    protected virtual void Awake()
    {
        health = GetComponent<Health>();
        if (health != null)
        {
            health.onDeath.AddListener(handleDeath);
        }
    }

    /// <summary>Interface implementation; forwards to the Health script.</summary>
    public virtual void takeDamage(int amount)
    {
        if (health != null)
        {
            float before = health.currentHealth;
            health.takeDamage(amount);
            float after = health.currentHealth;

            if (after < before)
            {
                onDamaged();
            }
        }
    }

    /// <summary>
    /// Hook called whenever <see cref="takeDamage(int)"/> actually reduced
    /// the health value.
    /// </summary>
    protected virtual void onDamaged() { }

    /// <summary>
    /// Called when the underlying <see cref="Health"/> component reports
    /// death. Derived classes should override and call base.onDeathHandler()
    /// if they still want the UnityEvent to fire.
    /// </summary>
    private void handleDeath()
    {
        onDeathHandler();
    }

    protected virtual void onDeathHandler()
    {
        onDeath?.Invoke();
    }
}

