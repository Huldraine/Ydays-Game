using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Vitesse")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float deceleration = 60f;

    [Header("Orientation")]
    [SerializeField] private bool flipSpriteOnDirection = true;

    private Rigidbody2D rb;
    private float inputX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Récupérer l'input horizontal (Q/D ou flèches gauche/droite)
        int x = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1; //Unity est sous qwerty donc A pour Q en azerty
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1;
        inputX = Mathf.Clamp(x, -1, 1);

        // Optionnel : retourner le sprite selon la direction
        if (flipSpriteOnDirection && inputX != 0f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * Mathf.Sign(inputX);
            transform.localScale = s;
        }
    }
    // Gestion du mouvement dans FixedUpdate pour la physique
    private void FixedUpdate()
    {
        Vector2 vel = rb.linearVelocity;
        float targetVX = inputX * maxSpeed;
        float rate = (Mathf.Abs(inputX) > 0.01f) ? acceleration : deceleration;
        vel.x = Mathf.MoveTowards(vel.x, targetVX, rate * Time.fixedDeltaTime);
        rb.linearVelocity = vel;
    }
}
