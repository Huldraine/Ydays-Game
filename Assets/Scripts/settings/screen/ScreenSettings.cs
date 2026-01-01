using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScreenSettings : MonoBehaviour
{
    Resolution[] resolutions;

    // Dropdown
    public TMP_Dropdown resolutionDropdown;

     public void Start ()
    {
        // Get all available resolutions and remove duplicates
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        // Clear old options of the dropdown
        resolutionDropdown.ClearOptions();

        // Create a list of options
        List<string> options = new List<string>();

        // Get current resolution index
        int currentResolutionIndex = 0;

        // Add resolutions to the options
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Check if this resolution is the current one
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to the dropdown
        resolutionDropdown.AddOptions(options);

        // Set the dropdown value to the current resolution
        resolutionDropdown.value = currentResolutionIndex;

        // Refresh the shown value
        resolutionDropdown.RefreshShownValue();

        // Set fullscreen auto
        Screen.fullScreen = true;
    }

    // Set resolution
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Set fullscreen mode

    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
