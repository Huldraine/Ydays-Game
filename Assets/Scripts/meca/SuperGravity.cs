using UnityEngine;


public class SuperGravity : MonoBehaviour
{
    [Header("param�tre de gravit�")]   
    public float normalGravityForce = 1f;    // gravit� normal normale
    public float superGravityForce = 5f;    // gravit� accentu� dans la zone 
    private Rigidbody2D rb;
    public bool inSuperGravityZone = false;

    // Start is called once before the first execution of Update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravityForce = rb.gravityScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("super_Gravity"))
        {
            inSuperGravityZone = true;
            rb.gravityScale = superGravityForce;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("super_Gravity"))
        {
            inSuperGravityZone = false;
            rb.gravityScale = normalGravityForce;
        }
    }
}