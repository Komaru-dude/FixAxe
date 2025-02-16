using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {   
        // Загружаем сцену
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // Извлекаем номер уровня из имени сцены (например, "Level_1" → 1)
        Match match = Regex.Match(currentSceneName, @"\d+");
        if (match.Success)
        {
            int currentLevel = int.Parse(match.Value);
            int nextLevel = currentLevel + 1;
            
            string nextSceneName = $"Level_{nextLevel}";

            // Проверяем, существует ли такая сцена в билде
            if (SceneExists(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("Следующий уровень отсутствует!");
            }
        }
        else
        {
            Debug.LogError("Не удалось определить номер уровня!");
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string extractedSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (extractedSceneName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
