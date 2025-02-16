using UnityEngine;

public class GraphyManager : MonoBehaviour
{
    [SerializeField] private GameObject graphyCanvas; // [Graphy] объект из сцены

    private void Start()
    {
        ApplyGraphySettings();
    }

    private void ApplyGraphySettings()
    {
        bool graphyEnabled = PlayerPrefs.GetInt("GraphyEnabled", 0) == 1;
        graphyCanvas.SetActive(graphyEnabled);
    }
}
