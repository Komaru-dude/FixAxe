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
        int screenFps = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
        int maxFps = screenFps;

#if UNITY_ANDROID
        int androidFps = 0;
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var windowManager = activity.Call<AndroidJavaObject>("getSystemService", "window"))
            using (var display = windowManager.Call<AndroidJavaObject>("getDefaultDisplay"))
            using (var mode = display.Call<AndroidJavaObject>("getMode"))
            {
                androidFps = Mathf.RoundToInt(mode.Call<float>("getRefreshRate"));
            }
        }
        catch
        {
            androidFps = 0;
        }

        maxFps = Mathf.Max(screenFps, androidFps);
#endif

        // Читаем настройку
        bool highFpsEnabled = PlayerPrefs.GetInt("HighFpsEnabled", 0) == 1;
        // Ставим поддерживаемый максимум
        Application.targetFrameRate = highFpsEnabled ? maxFps : 60;

        Debug.Log($"FPSManager: screen={screenFps}, target={Application.targetFrameRate}");
    }
}
