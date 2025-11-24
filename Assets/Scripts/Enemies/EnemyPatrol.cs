using UnityEngine;

/// <summary>
/// IA de patrouille + poursuite simple :
/// - patrouille de base en ligne droite
/// - tourne au bord des plateformes ou contre un mur
/// - optionnel : passe en mode chase si le joueur est à portée
/// </summary>
[RequireComponent(typeof(EnemyBase))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyPatrol : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase
    }

    [Header("Références")]
    [Tooltip("Transform du joueur. Si laissé vide, sera trouvé via le tag 'Player'.")]
    public Transform player;

    private EnemyBase enemyBase;
    private Rigidbody2D rb;
    private Collider2D col;

    [Header("Mouvement général")]
    [Tooltip("Vitesse de déplacement en patrouille.")]
    public float patrolSpeed = 2f;

    [Tooltip("Vitesse de déplacement en poursuite.")]
    public float chaseSpeed = 3.5f;

    [Tooltip("Direction de départ (1 = droite, -1 = gauche).")]
    [SerializeField] private int moveDirection = 1;

    [Tooltip("Flip automatique du sprite en fonction de la direction.")]
    public bool flipSpriteOnDirection = true;

    [Header("Détection du sol / murs")]
    [Tooltip("Layer du sol pour les raycasts.")]
    public LayerMask groundLayer;

    [Tooltip("Point de check pour le sol devant l'ennemi (à placer légèrement devant).")]
    public Transform groundCheck;

    [Tooltip("Distance verticale du raycast au sol.")]
    public float groundCheckDistance = 0.2f;

    [Tooltip("Point de check pour le mur devant l'ennemi (même position que groundCheck ou un peu plus haut).")]
    public Transform wallCheck;

    [Tooltip("Distance horizontale du raycast vers le mur.")]
    public float wallCheckDistance = 0.1f;

    [Header("Chase (poursuite du joueur)")]
    [Tooltip("Activer la poursuite du joueur ? Si false, l'ennemi fait seulement une patrouille.")]
    public bool enableChase = true;

    [Tooltip("Distance horizontale à partir de laquelle l'ennemi commence à poursuivre.")]
    public float chaseRange = 6f;

    [Tooltip("Distance horizontale à partir de laquelle l'ennemi abandonne la poursuite.")]
    public float loseChaseRange = 8f;

    [Tooltip("Tolérance en hauteur pour passer en mode chase (difference de Y).")]
    public float verticalChaseTolerance = 2f;

    [Header("Patrouille avancée")]
    [Tooltip("Temps d'arrêt quand l'ennemi se retourne (0 = pas d'arrêt).")]
    public float idleTimeOnTurn = 0f;

    private State state = State.Patrol;
    private float idleTimer = 0f;

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Empêche les rotations physiques
        rb.freezeRotation = true;

        // S'assure que moveDirection est bien 1 ou -1
        if (moveDirection == 0)
            moveDirection = 1;

        UpdateSpriteFlip();
    }

    private void FixedUpdate()
    {
        if (enemyBase == null || rb == null || col == null)
            return;

        // Mode idle (petite pause quand on tourne)
        if (idleTimer > 0f)
        {
            idleTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // Mise à jour de l'état (Patrol / Chase)
        UpdateState();

        // Mouvement selon l'état
        switch (state)
        {
            case State.Patrol:
                DoPatrol();
                break;
            case State.Chase:
                DoChase();
                break;
        }
    }

    private void UpdateState()
    {
        if (!enableChase || player == null)
        {
            state = State.Patrol;
            return;
        }

        float dx = player.position.x - transform.position.x;
        float absDx = Mathf.Abs(dx);
        float dy = Mathf.Abs(player.position.y - transform.position.y);

        // Critères pour entrer en chase
        bool canSeePlayer = absDx <= chaseRange && dy <= verticalChaseTolerance;

        // Critères pour sortir de chase
        bool lostPlayer = absDx >= loseChaseRange || dy > verticalChaseTolerance * 1.5f;

        if (state == State.Patrol && canSeePlayer)
        {
            state = State.Chase;
        }
        else if (state == State.Chase && lostPlayer)
        {
            state = State.Patrol;
        }
    }

    private void DoPatrol()
    {
        // Vérifie si on doit tourner (bord / mur)
        bool shouldTurn = ShouldTurn();

        if (shouldTurn)
        {
            TurnAround();
        }

        float speed = patrolSpeed;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveDirection * speed;
        rb.linearVelocity = velocity;
    }

    private void DoChase()
    {
        if (player == null)
        {
            DoPatrol();
            return;
        }

        // Détermine la direction à partir de la position du joueur
        float dx = player.position.x - transform.position.x;
        if (Mathf.Abs(dx) > 0.05f)
        {
            moveDirection = dx > 0f ? 1 : -1;
            UpdateSpriteFlip();
        }

        // Même logique de bord / mur que la patrouille
        bool shouldTurn = ShouldTurn();

        if (shouldTurn)
        {
            TurnAround();
        }

        float speed = chaseSpeed;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveDirection * speed;
        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// Renvoie true si l'ennemi est au bord d'une plateforme ou face à un mur.
    /// </summary>
    private bool ShouldTurn()
    {
        bool noGroundAhead = false;
        bool wallAhead = false;

        // Check sol
        if (groundCheck != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                groundCheck.position,
                Vector2.down,
                groundCheckDistance,
                groundLayer
            );
            noGroundAhead = (hit.collider == null);
        }

        // Check mur
        if (wallCheck != null)
        {
            Vector2 dir = new Vector2(moveDirection, 0f);
            RaycastHit2D hit = Physics2D.Raycast(
                wallCheck.position,
                dir,
                wallCheckDistance,
                groundLayer
            );
            wallAhead = (hit.collider != null);
        }

        return noGroundAhead || wallAhead;
    }

    /// <summary>
    /// Inverse la direction de déplacement + optionnellement un temps d'arrêt.
    /// </summary>
    private void TurnAround()
    {
        moveDirection = -moveDirection;
        UpdateSpriteFlip();

        if (idleTimeOnTurn > 0f)
        {
            idleTimer = idleTimeOnTurn;
        }
    }

    private void UpdateSpriteFlip()
    {
        if (!flipSpriteOnDirection) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection);
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Raycast sol
        if (groundCheck != null)
        {
            Gizmos.DrawLine(
                groundCheck.position,
                groundCheck.position + Vector3.down * groundCheckDistance
            );
        }

        // Raycast mur
        if (wallCheck != null)
        {
            Vector3 dir = Vector3.right * moveDirection;
            Gizmos.DrawLine(
                wallCheck.position,
                wallCheck.position + dir * wallCheckDistance
            );
        }

        // Ranges de chase
        if (enableChase)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, loseChaseRange);
        }
    }
}
