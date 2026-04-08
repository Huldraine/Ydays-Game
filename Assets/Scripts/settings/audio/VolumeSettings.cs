using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    private float currentVolume = 1f;

    public float getVolume()
    {
        return currentVolume;
    }

    public void setVolume(float volume)
    {
        currentVolume = volume;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
}
