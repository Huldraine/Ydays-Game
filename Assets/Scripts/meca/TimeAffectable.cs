using UnityEngine;

public class TimeAffectable : MonoBehaviour
{
    [Range(0f, 5f)]
    public float localTimeScale = 1f;

    public float GetLocalDeltaTime()
    {
        return Time.deltaTime * localTimeScale;
    }
}
