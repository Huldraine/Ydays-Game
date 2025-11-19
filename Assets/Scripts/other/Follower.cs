using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Player;      // le joueur à suivre
    public float speed = 2f;      // vitesse 
    public float maxDistance = 5f; // distance maximale pour suivre le joueur
    public float verif = 0f;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Player.position - transform.position;

        if (direction.sqrMagnitude > maxDistance*maxDistance )
        {
            verif = 1f;
        }

        if (verif ==1f)
        {
            transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
        }
    }
}
