using UnityEngine;


public class Gravity : MonoBehaviour
{
    [Header("paramêtre de gravité")]
    public float zeroGravityForce = -2f;   // gravité dans la zone 
    public float normalGravityForce = 1f;    // gravité normal normale
    public float superGravityForce = 5f;
    private Rigidbody2D rb;
    public bool inZeroGravityZone = false;
    public bool inSuperGravityZone = false;
    public bool inIntermittentGravityZone = false;
    public bool inIntermittentSuperGravityZone = false;
    public int indextimer = 1;
    public float timer = 10f;
    



    // Start is called once before the first execution of Update
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

            // On remet le timer à 10 secondes
            timer = 10f;

        }

        if (inIntermittentGravityZone)
        {
            rb.gravityScale = (indextimer == 1) ? zeroGravityForce : normalGravityForce;
        }

        if (inIntermittentSuperGravityZone)
        {
            rb.gravityScale = (indextimer == 1) ? superGravityForce : normalGravityForce;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("no_Gravity")) 
        {
            inZeroGravityZone = true;
            inSuperGravityZone = false;
            inIntermittentGravityZone = false;
            inIntermittentSuperGravityZone = false;
            rb.gravityScale = zeroGravityForce;
        }
        

        if (col.CompareTag("super_Gravity"))
        {
            inSuperGravityZone = true;
            inZeroGravityZone = false;
            inIntermittentGravityZone = false;
            inIntermittentSuperGravityZone = false;
            rb.gravityScale = superGravityForce;
        }

        if (col.CompareTag("no_Gravity_intermitant"))
        {
            inZeroGravityZone = false;
            inIntermittentGravityZone = true;
            inIntermittentSuperGravityZone = false;
            inSuperGravityZone = false;
            rb.gravityScale = zeroGravityForce;
        }

        if (col.CompareTag("super_Gravity_intermitant"))
        {
            inZeroGravityZone = false;
            inIntermittentGravityZone = false;
            inIntermittentSuperGravityZone = true;
            inSuperGravityZone = false;
            rb.gravityScale = superGravityForce;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("no_Gravity") || col.CompareTag("super_Gravity") || (col.CompareTag("no_Gravity_intermitant")) || (col.CompareTag("super_Gravity_intermitant")))
        {
            inZeroGravityZone = false;
            inSuperGravityZone = false;
            inIntermittentGravityZone = false;
            inIntermittentSuperGravityZone = false;
            rb.gravityScale = normalGravityForce;
        }
    }
}