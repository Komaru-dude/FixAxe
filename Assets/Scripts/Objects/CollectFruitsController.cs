using UnityEngine;

public class CFController : MonoBehaviour
{
    public Animator animator; // Ссылка на Animator
    public float destroyDelay = 0.5f; // Задержка перед уничтожением объекта

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Collected");
            
            int playerFruits = PlayerPrefs.GetInt("fruits", 0);
            playerFruits += 1;
            PlayerPrefs.SetInt("fruits", playerFruits);
            PlayerPrefs.Save();

            // Отладка
            Debug.Log("Fruits: " + PlayerPrefs.GetInt("fruits", 0));

            // Обновляем текст в UI через FManager
            FManager fManager = FindFirstObjectByType<FManager>(); // Новый способ вместо FindObjectOfType
            if (fManager != null)
            {
                fManager.LoadAndUpdateText();
            }
            else
            {
                Debug.LogWarning("FManager не найден!");
            }

            // Вызываем уничтожение объекта с задержкой
            Invoke("DestroyFruit", destroyDelay);
        }
    }

    void DestroyFruit()
    {
        Destroy(gameObject);
    }
}
