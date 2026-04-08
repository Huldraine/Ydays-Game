using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class that wraps a <see cref="Health"/> component and exposes
/// the IDamageable interface. It also fires a callback when the
/// associated health reaches zero.
/// Derived classes can override <see cref="OnDeath"/> to react to
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
            health.onDeath.AddListener(HandleDeath);
        }
    }

    /// <summary>Interface implementation; forwards to the Health script.</summary>
    public virtual void TakeDamage(int amount)
    {
        if (health != null)
        {
            float before = health.currentHealth;
            health.TakeDamage(amount);
            float after = health.currentHealth;

            if (after < before)
            {
                OnDamaged();
            }
        }
    }

    /// <summary>
    /// Hook called whenever <see cref="TakeDamage(int)"/> actually reduced
    /// the health value.
    /// </summary>
    protected virtual void OnDamaged() { }

    /// <summary>
    /// Called when the underlying <see cref="Health"/> component reports
    /// death. Derived classes should override and call base.OnDeath()
    /// if they still want the UnityEvent to fire.
    /// </summary>
    private void HandleDeath()
    {
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        onDeath?.Invoke();
    }
}
