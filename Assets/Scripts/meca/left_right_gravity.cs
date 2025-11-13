using UnityEngine;

public class LeftRightGravity : MonoBehaviour
{
    [Header("Paramètres de vecteur")]
    public float pushForce = 1f;
    private Rigidbody2D rb;
    public bool inVectorZoneRight = false;
    public bool inVectorZoneLeft = false;

    [Header("Paramètres de gravité")]
    public float zeroGravityForce = -0.5f;   // gravité dans la zone 
    public float normalGravityForce = 1f;    // gravité normale

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (inVectorZoneRight)
        {
            // Pousse vers la droite
            rb.velocity = new Vector2(pushForce, rb.velocity.y);
        }

        if (inVectorZoneLeft)
        {
            // Pousse vers la gauche
            rb.velocity = new Vector2(-pushForce, rb.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("left_Gravity"))
        {
            inVectorZoneLeft = true;
            inVectorZoneRight = false;
            rb.gravityScale = zeroGravityForce;
        }

        if (col.CompareTag("right_Gravity"))
        {
            inVectorZoneRight = true;
            inVectorZoneLeft = false;
            rb.gravityScale = zeroGravityForce;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("left_Gravity") || col.CompareTag("right_Gravity"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            rb.gravityScale = normalGravityForce;
        }
    }
}
