using UnityEngine;


public class rightGravity : MonoBehaviour
{
    [Header("parametre de vecteur")]
    public float pushrightForce = 1f;
    private Rigidbody2D rb;
    public bool inVectorZone = false;
    [Header("parametre de gravite")]
    public float zeroGravityForce = -0.5f;   // gravit� dans la zone 
    public float normalGravityForce = 1f;    // gravit� normal normale


    // Start is called once before the first execution of Update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        if (inVectorZone)
        {
            rb.linearVelocity = new Vector2(pushrightForce, rb.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("right_Gravity"))
        {
            inVectorZone = true;
            rb.gravityScale = zeroGravityForce;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("right_Gravity"))
        {
            inVectorZone = false;
            rb.gravityScale = normalGravityForce;
        }
    }
}






