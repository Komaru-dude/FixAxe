using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIManager uiManager;

    [SerializeField]
    private Canvas mainMenuCanvas;

    [SerializeField]
    private Canvas settingsCanvas;

    [SerializeField]
    private Canvas LevelSelectorCanvas;

    [SerializeField]
    private Grid gridd;

    public void SwitchToMainMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        if (gridd.gameObject.activeInHierarchy) {
            gridd.gameObject.SetActive(false);
        }
    }

    public void SwitchToSettings()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }

    public void SwitchToLevelSelector()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        LevelSelectorCanvas.gameObject.SetActive(true);
        gridd.gameObject.SetActive(true);
    }
}