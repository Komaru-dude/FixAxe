using UnityEngine;

public class FPSManager : MonoBehaviour
{
    private void Start()
    {
        ApplyFpsSettings();
    }

    private void ApplyFpsSettings()
    {
        bool highFpsEnabled = PlayerPrefs.GetInt("HighFpsEnabled", 0) == 1;
        Application.targetFrameRate = highFpsEnabled ? 120 : 60; // 120 FPS или 60 FPS
    }
}
