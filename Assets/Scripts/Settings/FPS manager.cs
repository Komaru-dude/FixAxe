using UnityEngine;

public class FPSManager : MonoBehaviour
{
    private void Start()
    {
        ApplyFpsSettings();
    }

    public static void ApplyFpsSettings()
    {
        // Получаем частоту экрана в Гц
        int maxRefreshRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);

        // Читаем настройку
        bool highFpsEnabled = PlayerPrefs.GetInt("HighFpsEnabled", 0) == 1;

        if (highFpsEnabled)
        {
            // Ставим максимум, который поддерживает экран
            Application.targetFrameRate = maxRefreshRate;
        }
        else
        {
            Application.targetFrameRate = 60;
        }
    }
}
