using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockForce = 5f;
    public Rigidbody2D rb;

    public void KnockFromEnemy(Vector2 enemyPos)
    {
        Vector2 dir = (transform.position - (Vector3)enemyPos).normalized;
        rb.AddForce(dir * knockForce, ForceMode2D.Impulse);
    }
}
