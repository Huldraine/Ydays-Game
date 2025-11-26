using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    private Animation anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.Play("Martial Hero");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.Play("jump");
        }
    }
}
