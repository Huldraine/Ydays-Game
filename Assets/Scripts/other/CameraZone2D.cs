using UnityEngine;

/// <summary>
/// Zone de caméra :
/// - optionnellement définit des bounds de caméra locaux (BoxCollider2D)
/// - peut ajouter / remplacer un offset de caméra
/// - s'active quand le joueur entre dans le trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CameraZone2D : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Contrôleur de caméra à piloter. Si vide, sera cherché automatiquement.")]
    public CameraController2D cameraController;

    [Header("Bounds de la zone")]
    [Tooltip("Bounds de caméra à utiliser dans cette zone. Si null, on garde les bounds globaux.")]
    public BoxCollider2D boundsOverride;

    [Header("Offset")]
    [Tooltip("Décalage de caméra spécifique à cette zone.")]
    public Vector2 zoneOffset = Vector2.zero;

    [Tooltip("Si vrai, remplace complètement l'offset de base de la caméra. Sinon, s'ajoute par dessus.")]
    public bool overrideOffset = false;

    private void Reset()
    {
        // On s'assure que le collider est bien un trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        if (cameraController == null)
        {
            cameraController = FindObjectOfType<CameraController2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (cameraController != null)
        {
            cameraController.SetActiveZone(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (cameraController != null)
        {
            cameraController.ClearActiveZone(this);
        }
    }
}
