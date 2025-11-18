using UnityEngine;

/// <summary>
/// Caméra 2D plus "calme" :
/// - suit le joueur avec smoothing séparé horizontal/vertical
/// - léger look ahead horizontal seulement quand le joueur se déplace
/// - clamp dans des bounds globaux ou de CameraZone2D
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController2D : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform target;          // Le Player
    [SerializeField] private Vector2 baseOffset = new Vector2(0f, 1.5f);

    [Header("Smoothing")]
    [Tooltip("Smoothing horizontal (plus petit = plus nerveux).")]
    [SerializeField] private float horizontalSmoothTime = 0.12f;
    [Tooltip("Smoothing vertical (plus grand = plus calme).")]
    [SerializeField] private float verticalSmoothTime = 0.20f;

    private float currentCamPosX;
    private float currentCamPosY;
    private float velocityX;
    private float velocityY;

    [Header("Look Ahead horizontal")]
    [SerializeField] private bool enableLookAhead = true;
    [SerializeField] private float lookAheadDistanceX = 1.5f;
    [SerializeField] private float moveThreshold = 0.5f;
    [SerializeField] private float lookAheadSmoothTime = 0.2f;

    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadVelocityX;

    [Header("Limites de la caméra")]
    [Tooltip("Bounds global de la caméra. Un BoxCollider2D qui englobe ton niveau.")]
    [SerializeField] private BoxCollider2D worldBounds;

    private Camera cam;
    private PlayerController2D playerController;
    private CameraZone2D activeZone;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (target != null)
        {
            playerController = target.GetComponent<PlayerController2D>();
        }

        // Initialise la position "lissée" pour éviter un gros snap au lancement
        currentCamPosX = transform.position.x;
        currentCamPosY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // --- Offset de base (modifié par zones éventuelles) ---
        Vector2 offset = baseOffset;
        BoxCollider2D boundsToUse = worldBounds;

        if (activeZone != null)
        {
            if (activeZone.overrideOffset)
                offset = activeZone.zoneOffset;
            else
                offset += activeZone.zoneOffset;

            if (activeZone.boundsOverride != null)
                boundsToUse = activeZone.boundsOverride;
        }

        // Cible brute de la caméra (sans look ahead)
        Vector3 focusPoint = target.position + (Vector3)offset;

        // --- Look ahead horizontal ---
        if (enableLookAhead && playerController != null)
        {
            float inputX = playerController.LastInputX;

            if (Mathf.Abs(inputX) > moveThreshold)
            {
                targetLookAheadX = lookAheadDistanceX * Mathf.Sign(inputX);
            }
            else
            {
                targetLookAheadX = 0f;
            }

            currentLookAheadX = Mathf.SmoothDamp(
                currentLookAheadX,
                targetLookAheadX,
                ref lookAheadVelocityX,
                lookAheadSmoothTime
            );

            focusPoint.x += currentLookAheadX;
        }

        // --- Smoothing séparé X / Y ---
        float targetX = focusPoint.x;
        float targetY = focusPoint.y;

        currentCamPosX = Mathf.SmoothDamp(
            currentCamPosX,
            targetX,
            ref velocityX,
            horizontalSmoothTime
        );

        currentCamPosY = Mathf.SmoothDamp(
            currentCamPosY,
            targetY,
            ref velocityY,
            verticalSmoothTime
        );

        Vector3 smoothedPos = new Vector3(currentCamPosX, currentCamPosY, transform.position.z);

        // --- Clamp dans les bounds (globaux ou zone) ---
        if (boundsToUse != null)
        {
            Bounds b = boundsToUse.bounds;

            float camHalfHeight = cam.orthographicSize;
            float camHalfWidth = camHalfHeight * cam.aspect;

            float minX = b.min.x + camHalfWidth;
            float maxX = b.max.x - camHalfWidth;
            float minY = b.min.y + camHalfHeight;
            float maxY = b.max.y - camHalfHeight;

            if (minX > maxX)
            {
                float midX = (b.min.x + b.max.x) * 0.5f;
                minX = maxX = midX;
            }

            if (minY > maxY)
            {
                float midY = (b.min.y + b.max.y) * 0.5f;
                minY = maxY = midY;
            }

            smoothedPos.x = Mathf.Clamp(smoothedPos.x, minX, maxX);
            smoothedPos.y = Mathf.Clamp(smoothedPos.y, minY, maxY);
        }

        transform.position = smoothedPos;
    }

    // ==== API pour les CameraZone2D ====
    public void SetActiveZone(CameraZone2D zone)
    {
        activeZone = zone;
    }

    public void ClearActiveZone(CameraZone2D zone)
    {
        if (activeZone == zone)
            activeZone = null;
    }

    // ==== API pour changer la cible ====
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
            playerController = target.GetComponent<PlayerController2D>();
        else
            playerController = null;
    }
}
