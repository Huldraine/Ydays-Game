using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    private float currentVolume = 1f;

    public float GetVolume()
    {
        return currentVolume;
    }

    public void SetVolume(float volume)
    {
        currentVolume = volume;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
}