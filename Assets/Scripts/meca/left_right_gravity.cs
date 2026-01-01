using UnityEngine;

public class LeftRightGravity : MonoBehaviour
{
    [Header("Paramètres de vecteur")]
    public float pushForce = 50f;

    public float pushForceIntermitant = 50f;

    private Rigidbody2D rb;
    public bool inVectorZoneRight = false;
    public bool inVectorZoneLeft = false;
    public bool inInterupteurVectorZoneLeft = false;
    public bool inInterupteurVectorZoneRight = false;
    public bool inIntermittentVectorZoneRight = false;
    public bool inIntermittentVectorZoneLeft = false;
    public int indextimer = 1;

    public float timer = 10f;

    [Header("Paramètres de gravité")]
    public float zeroGravityForce = -0.5f;   // gravité dans la zone 
    public float normalGravityForce = 1f;    // gravité normale

    public bool interupteuractif = false;
    public bool isInRange = false;
    public float pushForceInterupteur;


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

        if ( ((inIntermittentVectorZoneRight) && (indextimer == 1)) || ((inIntermittentVectorZoneLeft) && (indextimer == 1)) )
        {
            pushForceIntermitant = 50f;
            rb.gravityScale = zeroGravityForce;

        }

        if ( ( (inInterupteurVectorZoneRight) && (interupteuractif) ) || ( (inInterupteurVectorZoneLeft) && (interupteuractif) ) )
        {
            pushForceInterupteur = 50f;
            rb.gravityScale = zeroGravityForce;
        }
    }

    void FixedUpdate()
    {
        if (inVectorZoneRight)
        {
            // Pousse vers la droite
            rb.linearVelocity = new Vector2(pushForce, rb.linearVelocity.y);
            rb.gravityScale = zeroGravityForce;
        }

        if (inVectorZoneLeft)
        {
            rb.linearVelocity = new Vector2(-pushForce, rb.linearVelocity.y);
            rb.gravityScale = zeroGravityForce;
        }

        if (inIntermittentVectorZoneRight && indextimer == 1)
        {
            // Pousse vers la gauche
            rb.linearVelocity = new Vector2(pushForceIntermitant, rb.linearVelocity.y);
            rb.gravityScale = zeroGravityForce;
        }
        if (inIntermittentVectorZoneLeft && indextimer == 1)
        {
            // Pousse vers la gauche
            rb.linearVelocity = new Vector2(-pushForceIntermitant, rb.linearVelocity.y);
            rb.gravityScale = zeroGravityForce;
        }

        if (inInterupteurVectorZoneRight && interupteuractif == true)
        {
            // Pousse vers la gauche
            rb.linearVelocity = new Vector2(pushForceIntermitant, rb.linearVelocity.y);
            rb.gravityScale = zeroGravityForce;
        }
        if (inInterupteurVectorZoneLeft && interupteuractif == true)
        {
            // Pousse vers la gauche
            rb.linearVelocity = new Vector2(-pushForceIntermitant, rb.linearVelocity.y);
            rb.gravityScale = zeroGravityForce;
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
            inInterupteurVectorZoneLeft = false;
            inInterupteurVectorZoneRight = false;
  
        }

        if (col.CompareTag("right_Gravity"))
        {
            inVectorZoneRight = true;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = false;
            inInterupteurVectorZoneLeft = false;
            inInterupteurVectorZoneRight = false;

        }

        if (col.CompareTag("right_gravity_intermitant"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = true;
            inIntermittentVectorZoneLeft = false;
            inInterupteurVectorZoneLeft = false;
            inInterupteurVectorZoneRight = false;

        }

        if (col.CompareTag("left_gravity_intermitant"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = true;
            inInterupteurVectorZoneLeft = false;
            inInterupteurVectorZoneRight = false;

        }

        if (col.CompareTag("left_gravity_interupteur"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = false;
            inInterupteurVectorZoneLeft = true;
            inInterupteurVectorZoneRight = false;

        }

        if (col.CompareTag("right_gravity_interupteur"))
        {
            inVectorZoneRight = false;
            inVectorZoneLeft = false;
            inIntermittentVectorZoneRight = false;
            inIntermittentVectorZoneLeft = false;
            inInterupteurVectorZoneLeft = false;
            inInterupteurVectorZoneRight = true;

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
            inInterupteurVectorZoneLeft = false;
            inInterupteurVectorZoneRight = false;
            rb.gravityScale = normalGravityForce;


        }
    }

    private void Interupteur()
    {

        if (interupteuractif)
        {
            interupteuractif = false;
        }
        else
        {
            interupteuractif = true;
        }
    }
}
