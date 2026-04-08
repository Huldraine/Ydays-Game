using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Timing")]
    public float attackCooldown = 0.2f;
    private float cooldownTimer = 0f;

    [Header("Hitboxes")]
    public GameObject hitboxSide;
    public GameObject hitboxUp;
    public GameObject hitboxDown;

    [Header("Pogo")]
    public float pogoForce = 10f;

    public bool isFacingRight = true;

    private PlayerController2D playerController;

    private bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    // True uniquement pendant une attaque vers le bas
    private bool isDownAttack = false;
    public bool IsDownAttackActive => isDownAttack;

    void Awake()
    {
        playerController = GetComponent<PlayerController2D>();
    }

    void Start()
    {
        if (hitboxSide != null) hitboxSide.SetActive(false);
        if (hitboxUp != null) hitboxUp.SetActive(false);
        if (hitboxDown != null) hitboxDown.SetActive(false);
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && cooldownTimer <= 0f)
        {
            doAttack();
        }
    }

    void doAttack()
    {
        cooldownTimer = attackCooldown;

        float v = Input.GetAxisRaw("Vertical");

        if (v < 0f)
        {
            StartCoroutine(attackDown());
            return;
        }

        if (v > 0f)
        {
            StartCoroutine(attackUp());
            return;
        }

        StartCoroutine(attackSide());
    }

    IEnumerator attackSide()
    {
        isAttacking = true;
        isDownAttack = false;

        if (hitboxSide != null)
        {
            hitboxSide.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitboxSide.SetActive(false);
        }
        else
        {
            yield return null;
        }

        isAttacking = false;
        isDownAttack = false;
    }

    IEnumerator attackUp()
    {
        isAttacking = true;
        isDownAttack = false;

        if (hitboxUp != null)
        {
            hitboxUp.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitboxUp.SetActive(false);
        }
        else
        {
            yield return null;
        }

        isAttacking = false;
        isDownAttack = false;
    }

    IEnumerator attackDown()
    {
        isAttacking = true;
        isDownAttack = true;

        if (hitboxDown != null)
        {
            hitboxDown.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitboxDown.SetActive(false);
        }
        else
        {
            yield return null;
        }

        isAttacking = false;
        isDownAttack = false;
    }

    public void requestPogo()
    {
        if (playerController != null)
            playerController.applyPogo(pogoForce);
    }
}

