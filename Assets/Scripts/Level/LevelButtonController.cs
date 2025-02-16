using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButtonController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int levelNumber = 1;
    
    [Header("Colors")]
    [SerializeField] private Color completedColor = Color.green;
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;
    
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    private void Start()
    {
        if (!button) button = GetComponent<Button>();
        if (!buttonImage) buttonImage = GetComponent<Image>();
        
        InitializeButtonState();
        button.onClick.AddListener(OnLevelButtonClick);
    }

    private void InitializeButtonState()
    {
        bool isCompleted = PlayerPrefs.GetInt($"Level_{levelNumber}_Completed", 0) == 1;
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        if (isCompleted)
        {
            SetCompletedState();
        }
        else if (levelNumber <= unlockedLevels)
        {
            SetAvailableState();
        }
        else
        {
            SetLockedState();
        }
    }

    private void SetCompletedState()
    {
        buttonImage.color = completedColor;
        button.interactable = true;
    }

    private void SetAvailableState()
    {
        buttonImage.color = availableColor;
        button.interactable = true;
    }

    private void SetLockedState()
    {
        buttonImage.color = lockedColor;
        button.interactable = false;
    }

    private void OnLevelButtonClick()
    {
        SceneManager.LoadScene($"Level_{levelNumber}");
    }
}