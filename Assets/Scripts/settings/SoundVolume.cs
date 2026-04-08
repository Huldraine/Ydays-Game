using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Ajout pour la rÃ©fÃ©rence au Slider

public class SoundVolume : MonoBehaviour
{
    public AudioMixer masterMixer; // Votre rÃ©fÃ©rence Ã  l'AudioMixer
    public Slider volumeSlider;   // RÃ©fÃ©rence au composant Slider

    void Start()
    {
        // RÃ©cupÃ¨re la valeur actuelle du volume au dÃ©marrage
        loadVolumeFromMixer();
    }
    
    // Changer volume de l'AudioMixer
    public void setVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    
    // MAJ du Slider avec la valeur actuelle du volume
    private void loadVolumeFromMixer()
    {
        float mixerValue;
        // Tente d'obtenir la valeur exposÃ©e "MasterVolume"
        if (masterMixer.GetFloat("MasterVolume", out mixerValue))
        {
            // Conversion de la valeur logarithmique (dB) en une valeur linÃ©aire (0 Ã  1)
            float linearValue = Mathf.Pow(10, mixerValue / 20f); 
            
            // VÃ©rif si le Slider est assignÃ©
            if (volumeSlider != null)
            {
                // Applique la valeur lue au Slider
                volumeSlider.value = linearValue;
            }
        }
    }
}
