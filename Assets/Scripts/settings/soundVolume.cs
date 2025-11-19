using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Ajout pour la référence au Slider

public class soundVolume : MonoBehaviour
{
    public AudioMixer masterMixer; // Votre référence à l'AudioMixer
    public Slider volumeSlider;   // Référence au composant Slider

    void Start()
    {
        // Récupère la valeur actuelle du volume au démarrage
        LoadVolumeFromMixer();
    }
    
    // Changer volume de l'AudioMixer
    public void SetVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    
    // MAJ du Slider avec la valeur actuelle du volume
    private void LoadVolumeFromMixer()
    {
        float mixerValue;
        // Tente d'obtenir la valeur exposée "MasterVolume"
        if (masterMixer.GetFloat("MasterVolume", out mixerValue))
        {
            // Conversion de la valeur logarithmique (dB) en une valeur linéaire (0 à 1)
            float linearValue = Mathf.Pow(10, mixerValue / 20f); 
            
            // Vérif si le Slider est assigné
            if (volumeSlider != null)
            {
                // Applique la valeur lue au Slider
                volumeSlider.value = linearValue;
            }
        }
    }
}