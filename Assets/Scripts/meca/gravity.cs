using UnityEngine;


public class Gravity : MonoBehaviour
{
    [Header("paramêtre de gravité")]
    public float normalGravityForce = 1f;    // gravité normal normale
    public float superGravityForce = 5f;

    public float zeroGravityForce;    // gravité zéro
    public float fantomegravity;

    [Header("rigidbody")]
    private Rigidbody2D rb;

    [Header("paramêtre de zone")]
    private float zoneBottomY;
    private float zoneTopY;
    private float zoneMinX;
    private float zoneMaxX;
    private float enterHeight;

    [Header("zone de gravité")]
    public bool inZeroGravityZone = false;
    public bool inSuperGravityZone = false;
    public bool inIntermittentGravityZone = false;
    public bool inIntermittentSuperGravityZone = false;
    public bool inFantomeZone;

    [Header("index et timer")]
    public int indextimer = 1;
    public int indexsol = 0;
    public int indexverif = 0;
    public float timer = 10f;
    public float HightY;
    float LogGravity(float currentY, float bottomY, float topY, float targetGravity = -2f, float startGravity = -7f, float k = 9f)
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

        float currentY = transform.position.y;
        float currentX = transform.position.x;
        bool exitFantomeZoneDroite = currentY <= zoneTopY + enterHeight && currentY >= zoneTopY && currentX >= zoneMinX && currentX >= zoneMaxX;
        bool exitFantomeZoneGauche = currentY <= zoneTopY + enterHeight && currentY >= zoneTopY && currentX <= zoneMinX && currentX <= zoneMaxX;
        bool exitFantomeZoneHaut = currentY >= zoneTopY + enterHeight && currentY >= zoneTopY && currentX >= zoneMinX && currentX <= zoneMaxX;

        // Calcul de la zone fantôme
        inFantomeZone = currentY <= zoneTopY + enterHeight && currentY >= zoneTopY && currentX >= zoneMinX && currentX <= zoneMaxX;

        // Décrémenter le timer
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            indextimer = (indextimer == 0) ? 1 : 0;
            timer = 10f;
        }

        if (exitFantomeZoneDroite || exitFantomeZoneGauche || exitFantomeZoneHaut)
        {
            indexsol = 1;
        }

        // PRIORITÉ : zone fantôme
        if (inFantomeZone)
        {
            indexverif = 1;

            if (indexsol == 0)
            {
                rb.gravityScale = 7f;

                return;
            }

            if (indexsol == 1)
            {
                rb.gravityScale = normalGravityForce;
                return;
            }

        }
        else
        {
            indexverif = 0;

        }

        // Gravité selon la zone
        if (inZeroGravityZone)
        {
            float zeroGravityForce = LogGravity(currentY, zoneBottomY, zoneTopY);
            rb.gravityScale = zeroGravityForce;

        }
        else if (inIntermittentGravityZone)
        {
            float zeroGravityForce = LogGravity(currentY, zoneBottomY, zoneTopY);
            rb.gravityScale = (indextimer == 1) ? zeroGravityForce : normalGravityForce;

        }
        else if (inSuperGravityZone)
        {
            rb.gravityScale = superGravityForce;

        }
        else if (inIntermittentSuperGravityZone)
        {
            rb.gravityScale = (indextimer == 1) ? superGravityForce : normalGravityForce;

        }
        else
        {
            rb.gravityScale = normalGravityForce;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        zoneBottomY = col.bounds.min.y;
        zoneTopY = col.bounds.max.y;
        zoneMinX = col.bounds.min.x;
        zoneMaxX = col.bounds.max.x;

        HightY = zoneTopY - zoneBottomY;
        enterHeight = HightY / 2f; // <- toujours recalculé


        // Reset toutes les zones
        inZeroGravityZone = false;
        inIntermittentGravityZone = false;
        inSuperGravityZone = false;
        inIntermittentSuperGravityZone = false;

        // Activation selon tag
        if (col.CompareTag("no_Gravity"))
        {

            inZeroGravityZone = true;

            indexsol = 0;

        }
        else if (col.CompareTag("no_Gravity_intermitant"))
        {

            inIntermittentGravityZone = true;

            indexsol = 0;

        }
        else if (col.CompareTag("super_Gravity"))
        {

            inSuperGravityZone = true;

        }
        else if (col.CompareTag("super_Gravity_intermitant"))
        {

            inIntermittentSuperGravityZone = true;
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("no_Gravity") || col.CompareTag("no_Gravity_intermitant") || col.CompareTag("super_Gravity") || col.CompareTag("super_Gravity_intermitant"))
        {
            // Dès qu'on quitte le collider, reset complet
            rb.gravityScale = normalGravityForce;

            inZeroGravityZone = false;
            inIntermittentGravityZone = false;
            inSuperGravityZone = false;
            inIntermittentSuperGravityZone = false;
        }
    }
}
