using UnityEngine;
using UnityEngine.UI;

public class FpsSettings : MonoBehaviour
{
    [SerializeField] private Toggle highFpsToggle;

    private void Start()
    {
        highFpsToggle.isOn = PlayerPrefs.GetInt("HighFpsEnabled", 0) == 1;
        highFpsToggle.onValueChanged.AddListener(OnHighFpsToggleChanged);
        ApplyFpsSettings();
    }

    private void OnHighFpsToggleChanged(bool isEnabled)
    {
        PlayerPrefs.SetInt("HighFpsEnabled", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        ApplyFpsSettings();
    }

    private void ApplyFpsSettings()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("HighFpsEnabled", 0) == 1 ? 120 : 60;
    }
}
