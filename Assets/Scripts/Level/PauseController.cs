using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] 
    private Canvas LevelUiCanvas;

    [SerializeField]
    private Canvas PauseCanvas;

    public void PauseGame()
    {
        LevelUiCanvas.gameObject.SetActive(false);
        PauseCanvas.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        LevelUiCanvas.gameObject.SetActive(true);
        PauseCanvas.gameObject.SetActive(false);
    }
}