using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class DropdownSize : MonoBehaviour
{
    Resolution[] resolutions;

    // Dropdown
    public TMP_Dropdown resolutionDropdown;

    // Script ScreenSettings
    public ScreenSettings screenSettings;

    void Start()
    {
        // Get Dropdown
        resolutionDropdown = GetComponent<TMP_Dropdown>();

        // Script ScreenSettings
        screenSettings = ScreenSettings.Instance;
        if (screenSettings == null) return;

        // Get all available resolutions and remove duplicates
        resolutions = Screen.resolutions
            .Select(r => new Resolution { width = r.width, height = r.height })
            .Distinct()
            .ToArray();

        // Clear old options of the dropdown
        resolutionDropdown.ClearOptions();

        // Create a list of options
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
        }

        // Add options to the dropdown
        resolutionDropdown.AddOptions(options);

        // Set saved value without triggering event
        resolutionDropdown.SetValueWithoutNotify(screenSettings.currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        // Link dropdown to resolution change
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // Set resolution
    public void SetResolution(int resolutionIndex)
    {
        screenSettings.SetResolution(resolutionIndex);
    }

    void OnDestroy()
    {
        resolutionDropdown.onValueChanged.RemoveAllListeners();
    }
}
