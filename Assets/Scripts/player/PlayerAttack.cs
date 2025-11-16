using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackCooldown = 0.2f;
    private float cooldownTimer = 0f;

    public GameObject hitboxSide;
    public GameObject hitboxUp;
    public GameObject hitboxDown;

    public float pogoForce = 10f;
    public Rigidbody2D rb;

    public bool isFacingRight = true;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && cooldownTimer <= 0f)
        {
            DoAttack();
        }
    }

    void DoAttack()
    {
        cooldownTimer = attackCooldown;

        // Attaque vers le bas (pogo) si on descend
        if (Input.GetAxisRaw("Vertical") < 0 && rb.linearVelocity.y < 0)
        {
            StartCoroutine(AttackDown());
            return;
        }

        // Attaque vers le haut si on pousse haut
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            StartCoroutine(AttackUp());
            return;
        }

        // Sinon attaque côté
        StartCoroutine(AttackSide());
    }

    IEnumerator AttackSide()
    {
        hitboxSide.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitboxSide.SetActive(false);
    }

    IEnumerator AttackUp()
    {
        hitboxUp.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitboxUp.SetActive(false);
    }

    IEnumerator AttackDown()
    {
        hitboxDown.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitboxDown.SetActive(false);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, pogoForce);
    }
}
