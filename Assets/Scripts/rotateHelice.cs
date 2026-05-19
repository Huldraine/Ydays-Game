using UnityEngine;

public class rotateHelice : MonoBehaviour
{

    public float direction = 1f;

    private float timer;
    private float frameRate = 1f / 24f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;

            transform.Rotate(0f, 0f, 1f * Mathf.Sign(direction));
        }
    }
}