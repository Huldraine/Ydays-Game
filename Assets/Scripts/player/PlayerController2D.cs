using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerRespawn))]   // <-- nouveau : on s'assure qu'il y a un PlayerRespawn
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
    [SerializeField] private float fallMultiplier = 3.4f;      // Chute rapide
    [SerializeField] private float lowJumpMultiplier = 2.6f;   // Petit saut si relâché tôt
    [SerializeField] private float maxFallSpeed = -28f;        // Limite de chute
    [SerializeField] private float apexBoostThreshold = 0.35f; // Zone proche du sommet
    [SerializeField] private float apexBoostGravity = 1.2f;    // Gravité supplémentaire à l’apex

    [Header("Orientation")]
    [SerializeField] private bool flipSpriteOnDirection = true;

    [Header("Respawn Safe Position")]
    [SerializeField] private float safeOffsetX = 0.5f;   // combien on se recule sur X
    [SerializeField] private float safeOffsetY = 0.1f;   // petit décalage vers le haut


    [Header("Dash")]
    public float dash = 50f;
    public float direction = 1.0f;

    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerRespawn respawn;     // <-- nouveau

    private float inputX;

    // timers
    private float coyoteTimer;
    private float bufferTimer;

    // état du bouton saut
    private bool jumpHeld;
    private bool isJumping;
    private float jumpHoldTimer;

    // dernier input
    public float LastInputX => inputX;


    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        respawn = GetComponent<PlayerRespawn>();   // <-- nouveau

        // Important : règle le Rigidbody2D directement depuis le code
        rb.gravityScale = 1.4f;     // Gravité un peu plus forte pour éviter le “planage”
        rb.linearDamping = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
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

        if (Input.GetKey(KeyCode.E))
        {
            if (direction > 0f)
            {
                rb.linearVelocity = new Vector2(dash, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(-dash, rb.linearVelocity.y);
            }
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
        // ----- Détection du sol (boîte large sous les pieds) -----
        if (col != null)
        {
            Bounds b = col.bounds;
            Vector2 boxCenter = new Vector2(b.center.x, b.min.y - groundProbeSkin);
            Vector2 boxSize = new Vector2(b.size.x * groundProbeWidthScale, groundProbeHeight);
            isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer) != null;
        }

        // ----- mise à jour de la dernière position safe pour le respawn -----
        if (isGrounded && respawn != null)
        {
            // On part de la position actuelle
            Vector3 safePos = transform.position;

            // On regarde la direction dans laquelle le joueur se déplaçait
            float dir = inputX;
            if (Mathf.Abs(dir) < 0.01f)
            {
                // si le joueur ne bouge presque pas, on utilise la direction "facing"
                dir = direction;
            }

            // Si on a une vraie direction (gauche ou droite)
            if (Mathf.Abs(dir) > 0.01f)
            {
                // On recule un peu dans la direction opposée
                safePos.x -= Mathf.Sign(dir) * safeOffsetX;
            }

            // On le remet aussi un poil plus haut pour être sûr d'être sur la plateforme
            safePos.y += safeOffsetY;

            respawn.UpdateLastSafePosition(safePos);
        }


        // ----- Coyote + buffer -----
        if (isGrounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.fixedDeltaTime;
        bufferTimer -= Time.fixedDeltaTime;

        // ----- Mouvement horizontal -----
        Vector2 vel = rb.linearVelocity;
        float targetVX = inputX * maxSpeed;
        float rate = (Mathf.Abs(inputX) > 0.01f) ? acceleration : deceleration;
        vel.x = Mathf.MoveTowards(vel.x, targetVX, rate * Time.fixedDeltaTime);

        // ----- Déclenchement du saut (impulsion initiale) -----
        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            if (vel.y < 0f) vel.y = 0f;
            vel.y += jumpForce;

            isJumping = true;
            jumpHoldTimer = maxJumpHoldTime;

            bufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // ----- Maintien du saut -----
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

        // ----- Gravité avancée -----
        if (vel.y < 0f)
        {
            // Chute rapide
            vel.y += Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (vel.y > 0f && !jumpHeld && !isJumping)
        {
            // Petit saut si relâché tôt
            vel.y += Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // ----- Anti-planement -----
        if (!isGrounded && Mathf.Abs(vel.y) < apexBoostThreshold)
        {
            vel.y += Physics2D.gravity.y * apexBoostGravity * Time.fixedDeltaTime;
        }

        // Limite la vitesse de chute
        if (vel.y < maxFallSpeed)
            vel.y = maxFallSpeed;

        rb.linearVelocity = vel;
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
