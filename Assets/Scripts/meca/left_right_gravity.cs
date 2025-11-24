using UnityEngine;

public class LeftRightGravity : MonoBehaviour
{
    [Header("Paramètres de vecteur")]
    public float pushForce = 50f;

    public float pushForceIntermitant = 50f;

    private Rigidbody2D rb;
    public bool inVectorZoneRight = false;
    public bool inVectorZoneLeft = false;

    public bool inIntermittentVectorZoneRight = false;
    public bool inIntermittentVectorZoneLeft = false;
    public int indextimer = 1;

    public float timer = 10f;

    [Header("Paramètres de gravité")]
    public float zeroGravityForce = -0.5f;   // gravité dans la zone 
    public float normalGravityForce = 1f;    // gravité normale


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Décrémenter le timer
        timer -= Time.deltaTime;

        // Quand le timer atteint 0 ou moins
        if (timer <= 0f)
        {
            // On alterne entre 0 et 1
            indextimer = (indextimer == 0) ? 1 : 0;

            // On remet le timer à 5 secondes
            timer = 10f;

        }

        if (inIntermittentVectorZoneRight)
        {
            pushForceIntermitant = (indextimer == 1) ? 50f : 0f;

        }

        if (inIntermittentVectorZoneLeft)
        {
            pushForceIntermitant = (indextimer == 1) ? 50f : 0f;

        }
    }

    void FixedUpdate()
    {
        if (inVectorZoneRight)
        {
            // Pousse vers la droite
            rb.linearVelocity = new Vector2(pushForce, rb.linearVelocity.y);
        }

        if (inVectorZoneLeft)
        {
            rb.linearVelocity = new Vector2(-pushForce, rb.linearVelocity.y);
        }

        if (inIntermittentVectorZoneRight && indextimer == 1)
        {
            // Pousse vers la gauche
            rb.linearVelocity = new Vector2(pushForceIntermitant, rb.linearVelocity.y);
        }
        if (inIntermittentVectorZoneLeft && indextimer == 1)
        {
            // Pousse vers la gauche
            rb.linearVelocity = new Vector2(-pushForceIntermitant, rb.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("left_Gravity"))
        {
            inVectorZoneLeft = true;
            inVectorZoneRight = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = false;
            rb.gravityScale = zeroGravityForce;
        }

        if (col.CompareTag("right_Gravity"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = true;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = false;
            rb.gravityScale = zeroGravityForce;
        }

        if (col.CompareTag("right_gravity_intermitant"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = true;
            inIntermittentVectorZoneLeft = false;
            rb.gravityScale = zeroGravityForce;
        }

        if (col.CompareTag("left_gravity_intermitant"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = true;
            rb.gravityScale = zeroGravityForce;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("left_Gravity") || col.CompareTag("right_Gravity") || col.CompareTag("left_gravity_intermitant") || col.CompareTag("right_gravity_intermitant"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = false;
            rb.gravityScale = normalGravityForce;
            
        }
    }
}
