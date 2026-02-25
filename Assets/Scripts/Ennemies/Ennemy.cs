using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public Transform player;
    
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Stats")]
    public int health = 5;
    public int attackDamage = 1;
    public float attackSpeed = 1.5f; 
    public float moveSpeed = 2.5f;
    public float stoppingDistance = 1.2f;

    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMovement();
            TryAttack();
        }
    }

    void MoveTowardsPlayer()
    {
        float directionX = (player.position.x - transform.position.x);
        directionX = Mathf.Sign(directionX);

        rb.linearVelocity = new Vector2(directionX * moveSpeed, rb.linearVelocity.y);

        if (directionX > 0) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
    }

    void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackSpeed)
        {
            Debug.Log("L'ennemi attaque !");
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}