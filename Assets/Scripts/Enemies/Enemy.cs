using UnityEngine;

public class Enemy : EnemyBase
{
    public Transform player;

    [Header("Stats")]
    public int attackDamage = 1;
    public float attackSpeed = 1.5f;
    public float moveSpeed = 2.5f;
    public float stoppingDistance = 1.2f;
    private float lastAttackTime;
    public float hoverHeight = 4f;

    protected override void Start()
    {
        base.Start();

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            moveTowardsPlayer();
        }
        else
        {
            stopMovement();
            tryAttack();
        }
    }

    void moveTowardsPlayer()
    {
        float currentYOffset = (Time.time >= lastAttackTime + attackSpeed) ? 0f : hoverHeight;
        Vector2 targetPosition;

        if (hoverHeight > 0)
        {
            targetPosition = new Vector2(player.position.x, player.position.y + currentYOffset);
        }
        else
        {
            targetPosition = new Vector2(player.position.x, transform.position.y);
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < 0) transform.localScale = new Vector3(1, 1, 1);
    }

    void stopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    void tryAttack()
    {
        if (Time.time >= lastAttackTime + attackSpeed)
        {
            Debug.Log("Attaque");
            lastAttackTime = Time.time;
        }
    }
}