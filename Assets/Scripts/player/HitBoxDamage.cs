using UnityEngine;

public class HitBoxDamage : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        Follower enemy = other.GetComponent<Follower>();
        if (enemy != null)
        {
            enemy.TakeDamage(1); // 1 point de dégâts
        }
    }
}
