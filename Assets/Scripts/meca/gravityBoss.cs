using UnityEngine;

public class gravityBoss : MonoBehaviour
{
    [Header("paramêtre de gravité")]
    public float normalGravityForce = 1f;    // gravité normal normale
    public float superGravityForce = 5f;
    public float zeroGravityForce;    // gravité zéro

    public float pushForce = 10f;

    [Header("rigidbody")]
    private Rigidbody2D rb;

    [Header("zone de gravité")]
    public bool inZeroGravityZone = false;
    public bool inSuperGravityZone = false;
    public bool inVectorZoneRight = false;
    public bool inVectorZoneLeft = false;

    [Header("zone paramètres")]
    [SerializeField] private float currentY;
    [SerializeField] private float zoneBottomY;
    [SerializeField] private float zoneTopY;
    [SerializeField] private float zoneMinX;
    [SerializeField] private float zoneMaxX;
    [SerializeField] private float milieu;
    [SerializeField] private bool milieuhaut;
    [SerializeField] private bool milieuBas;
    [SerializeField] private float HightY;
    [SerializeField] private float enterHeight;

    float logGravity(float currentY, float bottomY, float topY, float targetGravity = -0.5f, float startGravity = -5f, float k = 9f)
    {
        float height = Mathf.Max(0.0001f, topY - bottomY);
        float t = Mathf.Clamp01((currentY - bottomY) / height);

        // Logarithme inversé pour que la gravité reste forte longtemps
        float logT = 1f - Mathf.Log(1f + k * (1f - t)) / Mathf.Log(1f + k);

        return startGravity + logT * (targetGravity - startGravity);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentY = transform.position.y;
        milieuBas = currentY < milieu && currentY > zoneBottomY;
        milieuhaut = currentY > milieu && currentY < zoneTopY;
    }

    void FixedUpdate()
    {
        if (inZeroGravityZone)
        {
            rb.gravityScale = logGravity(currentY,zoneTopY,zoneBottomY);
        }
        else if (inSuperGravityZone)
        {
            rb.gravityScale = superGravityForce;
        }

        if (inVectorZoneRight)
        {
            if (milieuhaut)
            {
                rb.linearVelocity = new Vector2(pushForce, rb.linearVelocity.y - 5);
            }
            if (milieuBas)
            {
                rb.linearVelocity = new Vector2(pushForce, rb.linearVelocity.y + 5);
            }
        }

        if (inVectorZoneLeft)
        {
            if (milieuhaut)
            {
                rb.linearVelocity = new Vector2(-pushForce, rb.linearVelocity.y - 5);
            }
            if (milieuBas)
            {
                rb.linearVelocity = new Vector2(-pushForce, rb.linearVelocity.y + 5);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        zoneBottomY = col.bounds.min.y;
        zoneTopY = col.bounds.max.y;
        zoneMinX = col.bounds.min.x;
        zoneMaxX = col.bounds.max.x;
        milieu = (zoneTopY + zoneBottomY)/2f ;

        HightY = zoneTopY - zoneBottomY;
        enterHeight = HightY / 2f; // <- toujours recalculé

        inZeroGravityZone = false;
        inSuperGravityZone = false;
        inVectorZoneLeft = false;
        inVectorZoneRight = false;

        // Activation selon tag
        if (col.CompareTag("topBossgravity"))
        {
            inZeroGravityZone = true;
            inVectorZoneLeft = false;
            inVectorZoneLeft = false;
            inVectorZoneRight = false;

        }
        else if (col.CompareTag("bottomBossgravity"))
        {
            inZeroGravityZone = false;
            inSuperGravityZone = true;
            inVectorZoneLeft = false;
            inVectorZoneRight = false;
        }       


        if (col.CompareTag("leftBossgravity"))
        {
            inZeroGravityZone = false;
            inSuperGravityZone = false;
            inVectorZoneLeft = true;
            inVectorZoneRight = false;
        }

        if (col.CompareTag("rightBossgravity"))
        {
            inZeroGravityZone = false;
            inSuperGravityZone = false;
            inVectorZoneLeft = false;
            inVectorZoneRight = true;
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("topBossgravity")  || col.CompareTag("bottomBossgravity") || col.CompareTag("leftBossgravity")  || col.CompareTag("rightBossgravity"))
        {
            rb.gravityScale = normalGravityForce;
            inZeroGravityZone = false;
            inSuperGravityZone = false;
            inVectorZoneLeft = false;
            inVectorZoneRight = false;
        }
    }
}