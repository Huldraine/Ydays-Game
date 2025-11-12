using UnityEngine;


public class Gravity : MonoBehaviour
{
    [Header("paramêtre de gravité")]
    public float zeroGravityForce = -2f;   // gravité dans la zone 
    public float normalGravityForce = 1f;    // gravité normal normale
    private Rigidbody2D rb;
    public bool inZeroGravityZone = false;

    // Start is called once before the first execution of Update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("no_Gravity"))
        {
            inZeroGravityZone = true;
            rb.gravityScale = zeroGravityForce;
        }
        

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("no_Gravity"))
        {
            inZeroGravityZone = false;
            rb.gravityScale = normalGravityForce;
        }
    }
}