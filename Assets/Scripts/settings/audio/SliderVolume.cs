using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    private Slider slider;
    private VolumeSettings volumeSettings;

    void Start()
    {
        slider = GetComponent<Slider>();

        // Get VolumeSettings from GameManager
        volumeSettings = GameManager.Instance.GetComponentInChildren<VolumeSettings>();

        if (volumeSettings == null)
        {
            return;
        }

        // Initialize slider value without triggering OnValueChanged
        slider.SetValueWithoutNotify(volumeSettings.GetVolume());

        // Link OnValueChanged to VolumeSettings
        slider.onValueChanged.AddListener(volumeSettings.SetVolume);
    }

    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(volumeSettings.SetVolume);
    }
}
