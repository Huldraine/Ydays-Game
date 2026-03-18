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
    public float hoverHeight = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
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
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y + hoverHeight);
        Vector2 direction = ((Vector2)transform.position - targetPosition).normalized;
        
        rb.linearVelocity = -direction * moveSpeed;

        if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
    }

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
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
        if (health <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}