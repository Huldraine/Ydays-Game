using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerRespawn))]
[RequireComponent(typeof(Health))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Vitesse")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float acceleration = 55f;
    [SerializeField] private float deceleration = 85f;

    [Header("Saut - impulsion initiale")]
    [SerializeField] private float jumpForce = 8.5f;

    [Header("Saut - maintien (hauteur variable)")]
    [SerializeField] private float maxJumpHoldTime = 0.13f;
    [SerializeField] private float holdBoostAccel = 20f;

    [Header("Détection du sol")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundProbeSkin = 0.02f;
    [SerializeField] private float groundProbeHeight = 0.12f;
    [SerializeField] private float groundProbeWidthScale = 0.9f;

    [Header("Confort")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBuffer = 0.12f;

    [Header("Gravité avancée")]
    [SerializeField] private float fallMultiplier = 3.4f;
    [SerializeField] private float lowJumpMultiplier = 2.6f;
    [SerializeField] private float maxFallSpeed = -28f;
    [SerializeField] private float apexBoostThreshold = 0.35f;
    [SerializeField] private float apexBoostGravity = 1.2f;

    [Header("Orientation")]
    [SerializeField] private bool flipSpriteOnDirection = true;

    [Header("Respawn Safe Position")]
    [SerializeField] private float safeOffsetX = 0.5f;
    [SerializeField] private float safeOffsetY = 0.1f;

    [Header("Dash")]
    public float dash = 50f;
    public float direction = 1.0f;

    [Header("Hit / Knockback")]
    [SerializeField] private float knockbackForceX = 10f;
    [SerializeField] private float knockbackForceY = 6f;
    [SerializeField] private float knockbackDuration = 0.15f;
    [SerializeField] private float invincibilityTime = 1.0f;

    [Header("Invincibilité & collisions")]
    [Tooltip("Nom du layer utilisé par les ennemis (doit exister dans Unity).")]
    [SerializeField] private string enemyLayerName = "Enemy";

    [Tooltip("Tag utilisé par les ennemis (pour ignorer les collisions individuellement).")]
    [SerializeField] private string enemyTagName = "Enemy";

    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerRespawn respawn;
    private Health health;
    private SpriteRenderer spriteRenderer;

    private float inputX;

    // timers saut
    private float coyoteTimer;
    private float bufferTimer;

    private bool jumpHeld;
    private bool isJumping;
    private float jumpHoldTimer;

    public float LastInputX => inputX;

    private bool isGrounded;

    // Knockback & invincibilité
    private bool isKnockedBack;
    private float knockbackTimer;

    private bool isInvincible;
    private float invincibilityTimer;
    private float flashTimer;
    private float flashInterval = 0.1f;

    // Layers
    private int playerLayerIndex;
    private int enemyLayerIndex;

    /// <summary>Vitesse verticale actuelle du joueur.</summary>
    public float VerticalVelocity => rb != null ? rb.linearVelocity.y : 0f;
    public bool IsInvincible => isInvincible;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        respawn = GetComponent<PlayerRespawn>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rb.gravityScale = 1.4f;
        rb.linearDamping = 0f;
        rb.freezeRotation = true;

        // Layers
        playerLayerIndex = gameObject.layer;
        enemyLayerIndex = LayerMask.NameToLayer(enemyLayerName);
        if (enemyLayerIndex < 0)
        {
            Debug.LogWarning($"[PlayerController2D] Layer '{enemyLayerName}' introuvable. Vérifie son nom dans les Layer Settings.");
        }
    }

    private void Update()
    {
        HandleInvincibilityTimers();

        if (isKnockedBack)
            return;

        // --- Input horizontal ---
        int x = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            x -= 1;
            direction = -1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            x += 1;
            direction = 1f;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (direction > 0f)
                rb.linearVelocity = new Vector2(dash, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(-dash, rb.linearVelocity.y);
        }

        inputX = Mathf.Clamp(x, -1, 1);

        // --- Saut : buffer + état maintenu ---
        if (Input.GetKeyDown(KeyCode.Space))
            bufferTimer = jumpBuffer;

        jumpHeld = Input.GetKey(KeyCode.Space);

        // --- Flip du sprite ---
        if (flipSpriteOnDirection && inputX != 0f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * Mathf.Sign(inputX);
            transform.localScale = s;
        }
    }

    private void FixedUpdate()
    {
        // Détection du sol
        if (col != null)
        {
            Bounds b = col.bounds;
            Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundProbeSkin);
            Vector2 boxSize = new Vector2(b.size.x * groundProbeWidthScale, groundProbeHeight);
            isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer) != null;
        }

        // Safe position
        if (isGrounded && respawn != null)
        {
            Vector3 safePos = transform.position;

            float dir = inputX;
            if (Mathf.Abs(dir) < 0.01f)
                dir = direction;

            if (Mathf.Abs(dir) > 0.01f)
                safePos.x -= Mathf.Sign(dir) * safeOffsetX;

            safePos.y += safeOffsetY;
            respawn.UpdateLastSafePosition(safePos);
        }

        // Coyote / buffer
        if (isGrounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.fixedDeltaTime;
        bufferTimer -= Time.fixedDeltaTime;

        Vector2 vel = rb.linearVelocity;

        if (!isKnockedBack)
        {
            // Mouvement horizontal
            float targetVX = inputX * maxSpeed;
            float rate = (Mathf.Abs(inputX) > 0.01f) ? acceleration : deceleration;
            vel.x = Mathf.MoveTowards(vel.x, targetVX, rate * Time.fixedDeltaTime);

            // Déclenchement du saut
            if (bufferTimer > 0f && coyoteTimer > 0f)
            {
                if (vel.y < 0f) vel.y = 0f;
                vel.y += jumpForce;

                isJumping = true;
                jumpHoldTimer = maxJumpHoldTime;

                bufferTimer = 0f;
                coyoteTimer = 0f;
            }

            // Maintien du saut
            if (isJumping)
            {
                if (jumpHeld && jumpHoldTimer > 0f)
                {
                    vel.y += holdBoostAccel * Time.fixedDeltaTime;
                    jumpHoldTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
        }
        else
        {
            // Timer de knockback
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
        }

        // Gravité avancée
        if (vel.y < 0f)
        {
            vel.y += Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (vel.y > 0f && !jumpHeld && !isJumping)
        {
            vel.y += Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }

        if (!isGrounded && Mathf.Abs(vel.y) < apexBoostThreshold)
            vel.y += Physics2D.gravity.y * apexBoostGravity * Time.fixedDeltaTime;

        if (vel.y < maxFallSpeed)
            vel.y = maxFallSpeed;

        rb.linearVelocity = vel;
    }

    /// <summary>
    /// Appelé quand un ennemi touche le joueur : dégâts + knockback + invincibilité.
    /// </summary>
    public void OnHitByEnemy(Vector2 enemyPosition, int damage)
    {
        if (isInvincible || health == null)
            return;

        // Dégâts
        health.TakeDamage(damage);

        // Invincibilité (et ignore collisions avec ennemis)
        SetInvincibleState(true);
        invincibilityTimer = invincibilityTime;
        flashTimer = 0f;

        // Knockback
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        float dir = Mathf.Sign(transform.position.x - enemyPosition.x);
        if (dir == 0f)
        {
            dir = Mathf.Sign(direction);
            if (dir == 0f) dir = 1f;
        }

        Vector2 vel = rb.linearVelocity;
        vel.x = dir * knockbackForceX;
        vel.y = knockbackForceY;
        rb.linearVelocity = vel;
    }

    /// <summary>Applique un pogo (rebond vers le haut).</summary>
    public void ApplyPogo(float pogoForce)
    {
        Vector2 vel = rb.linearVelocity;

        if (vel.y < 0f)
            vel.y = 0f;

        vel.y = pogoForce;
        rb.linearVelocity = vel;

        isJumping = false;
        jumpHoldTimer = 0f;
    }

    private void HandleInvincibilityTimers()
    {
        if (!isInvincible)
            return;

        invincibilityTimer -= Time.deltaTime;
        if (invincibilityTimer <= 0f)
        {
            SetInvincibleState(false);
            return;
        }

        // Clignotement
        if (spriteRenderer != null)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                flashTimer = flashInterval;
            }
        }
    }

    /// <summary>
    /// Active/désactive l'invincibilité + ignore collisions avec les ennemis.
    /// </summary>
    private void SetInvincibleState(bool value)
    {
        isInvincible = value;

        // 1) Ignore collisions par layer (si le layer Enemy existe)
        if (enemyLayerIndex >= 0)
        {
            Physics2D.IgnoreLayerCollision(playerLayerIndex, enemyLayerIndex, value);
        }

        // 2) Ignore collisions directes avec chaque collider taggé Enemy
        if (!string.IsNullOrEmpty(enemyTagName))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTagName);
            foreach (GameObject enemy in enemies)
            {
                Collider2D enemyCol = enemy.GetComponent<Collider2D>();
                if (enemyCol != null && col != null)
                {
                    Physics2D.IgnoreCollision(col, enemyCol, value);
                }
            }
        }

        // Quand on sort de l'invincibilité, on remet le sprite visible
        if (!value && spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null && !TryGetComponent(out col)) return;

        Bounds b = col.bounds;
        Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundProbeSkin);
        Vector2 boxSize = new Vector2(b.size.x * groundProbeWidthScale, groundProbeHeight);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}