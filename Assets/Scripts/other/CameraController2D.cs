using UnityEngine;

/// <summary>
/// Camï¿½ra 2D "calme" :
/// - follow avec smoothing sï¿½parï¿½ horizontal/vertical
/// - lï¿½ger look ahead horizontal
/// - dead zone verticale : la camï¿½ra ne bouge pas en Y tant que le joueur reste dans une zone centrale
/// - clamp dans des bounds globaux ou de CameraZone2D.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController2D : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 baseOffset = new Vector2(0f, 1.5f);

    [Header("Smoothing")]
    [Tooltip("Smoothing horizontal (plus petit = plus nerveux).")]
    [SerializeField] private float horizontalSmoothTime = 0.12f;
    [Tooltip("Smoothing vertical (plus grand = plus calme).")]
    [SerializeField] private float verticalSmoothTime = 0.25f;

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

    [Header("Dead zone verticale")]
    [Tooltip("Taille de la zone verticale dans laquelle la camï¿½ra ne bouge presque pas.")]
    [SerializeField] private float verticalDeadZone = 1.0f;

    [Header("Limites de la camï¿½ra")]
    [Tooltip("Bounds global de la camï¿½ra. Un BoxCollider2D qui englobe ton niveau.")]
    [SerializeField] private BoxCollider2D worldBounds;

    private Camera cam;
    private PlayerController2D playerController;
    private CameraZone2D activeZone;

    private void getCurrentOffsetAndBounds(out Vector2 offset, out BoxCollider2D boundsToUse)
    {
        offset = baseOffset;
        boundsToUse = worldBounds;

        if (activeZone != null)
        {
            if (activeZone.overrideOffset)
                offset = activeZone.zoneOffset;
            else
                offset += activeZone.zoneOffset;

            if (activeZone.boundsOverride != null)
                boundsToUse = activeZone.boundsOverride;
        }
    }

    private Vector3 clampToBounds(Vector3 position, BoxCollider2D boundsToUse)
    {
        if (boundsToUse == null)
            return position;

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

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        return position;
    }

    private bool tryResolveTarget()
    {
        if (target != null)
        {
            if (playerController == null)
                playerController = target.GetComponent<PlayerController2D>();
            return true;
        }

        PlayerController2D foundPlayer = FindAnyObjectByType<PlayerController2D>();
        if (foundPlayer != null)
        {
            setTarget(foundPlayer.transform);
            return true;
        }

        GameObject taggedPlayer = GameObject.FindWithTag("Player");
        if (taggedPlayer != null)
        {
            setTarget(taggedPlayer.transform);
            return true;
        }

        return false;
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();

        tryResolveTarget();

        currentCamPosX = transform.position.x;
        currentCamPosY = transform.position.y;

        if (target != null)
            snapToTargetInstant();
    }

    private void LateUpdate()
    {
        if (!tryResolveTarget())
            return;

        // --- Offset de base + ï¿½ventuelle CameraZone ---
        getCurrentOffsetAndBounds(out Vector2 offset, out BoxCollider2D boundsToUse);

        Vector3 focusPoint = target.position + (Vector3)offset;

        // --- Look ahead horizontal ---
        if (enableLookAhead && playerController != null)
        {
            float inputX = playerController.LastInputX;

            if (Mathf.Abs(inputX) > moveThreshold)
                targetLookAheadX = lookAheadDistanceX * Mathf.Sign(inputX);
            else
                targetLookAheadX = 0f;

            currentLookAheadX = Mathf.SmoothDamp(
                currentLookAheadX,
                targetLookAheadX,
                ref lookAheadVelocityX,
                lookAheadSmoothTime
            );

            focusPoint.x += currentLookAheadX;
        }

        // --- Cibles X/Y brutes ---
        float targetX = focusPoint.x;
        float targetY = focusPoint.y;

        // --- Dead zone verticale ---
        // Tant que le joueur reste ï¿½ l'intï¿½rieur d'une bande horizontale autour de la camï¿½ra,
        // on ne bouge pas (ou trï¿½s peu) la camï¿½ra en Y.
        float dy = targetY - currentCamPosY;

        if (Mathf.Abs(dy) < verticalDeadZone)
        {
            // On "freeze" la cible verticale : la camï¿½ra reste lï¿½
            targetY = currentCamPosY;
        }
        else
        {
            // Option : on peut commencer ï¿½ bouger la camï¿½ra en poussant lï¿½gï¿½rement
            // targetY vers l'extï¿½rieur de la dead zone, mais ici on laisse la valeur brute
        }

        // --- Smoothing sï¿½parï¿½ X / Y ---
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

        // --- Clamp dans les bounds ---
        smoothedPos = clampToBounds(smoothedPos, boundsToUse);

        transform.position = smoothedPos;
    }

    // ==== API pour les CameraZone2D ====
    public void setActiveZone(CameraZone2D zone)
    {
        activeZone = zone;
    }

    public void clearActiveZone(CameraZone2D zone)
    {
        if (activeZone == zone)
            activeZone = null;
    }

    // ==== API pour changer la cible ====
    public void setTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            playerController = target.GetComponent<PlayerController2D>();
            currentCamPosX = transform.position.x;
            currentCamPosY = transform.position.y;
            snapToTargetInstant();
        }
        else
            playerController = null;
    }

    public void snapToTargetInstant(bool resetLookAhead = true)
    {
        if (!tryResolveTarget())
            return;

        if (resetLookAhead)
        {
            currentLookAheadX = 0f;
            targetLookAheadX = 0f;
            lookAheadVelocityX = 0f;
        }

        getCurrentOffsetAndBounds(out Vector2 offset, out BoxCollider2D boundsToUse);

        Vector3 pos = target.position + (Vector3)offset;
        pos.z = transform.position.z;
        pos = clampToBounds(pos, boundsToUse);

        transform.position = pos;
        currentCamPosX = pos.x;
        currentCamPosY = pos.y;
        velocityX = 0f;
        velocityY = 0f;
    }
}

