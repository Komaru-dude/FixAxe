using UnityEngine;
using UnityEngine.UI;

public class GraphySettings : MonoBehaviour
{
    [SerializeField] private Toggle graphyToggle; // Галочка в UI

    private void Start()
    {
        // Загружаем сохранённое состояние
        graphyToggle.isOn = PlayerPrefs.GetInt("GraphyEnabled", 0) == 1;
        graphyToggle.onValueChanged.AddListener(OnGraphyToggleChanged);
    }

    private void OnGraphyToggleChanged(bool isEnabled)
    {
        PlayerPrefs.SetInt("GraphyEnabled", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
}
