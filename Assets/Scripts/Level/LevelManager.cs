using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevelNumber = 1;

    // Вызывается при успешном завершении уровня
    public void CompleteLevel()
    {
        // Сохраняем статус завершения текущего уровня
        PlayerPrefs.SetInt($"Level_{currentLevelNumber}_Completed", 1);

        // Обновляем количество открытых уровней
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
        if (currentLevelNumber >= unlockedLevels)
        {
            PlayerPrefs.SetInt("UnlockedLevels", currentLevelNumber + 1);
        }

        PlayerPrefs.Save();
    }
}