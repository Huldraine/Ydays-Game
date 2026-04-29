using UnityEngine;

/// <summary>
/// Zone de cam�ra :
/// - optionnellement d�finit des bounds de cam�ra locaux (BoxCollider2D)
/// - peut ajouter / remplacer un offset de cam�ra
/// - s'active quand le joueur entre dans le trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CameraZone2D : MonoBehaviour
{
    [Header("R�f�rences")]
    [Tooltip("Contr�leur de cam�ra � piloter. Si vide, sera cherch� automatiquement.")]
    public CameraController2D cameraController;

    [Header("Bounds de la zone")]
    [Tooltip("Bounds de cam�ra � utiliser dans cette zone. Si null, on garde les bounds globaux.")]
    public BoxCollider2D boundsOverride;

    [Header("Offset")]
    [Tooltip("D�calage de cam�ra sp�cifique � cette zone.")]
    public Vector2 zoneOffset = Vector2.zero;

    [Tooltip("Si vrai, remplace compl�tement l'offset de base de la cam�ra. Sinon, s'ajoute par dessus.")]
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
            cameraController = FindFirstObjectByType<CameraController2D>();
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
