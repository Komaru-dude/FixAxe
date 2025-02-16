using UnityEngine;

// Оригинальный код взят отсюда: https://github.com/parwam/Pixel-Adventure/blob/main/Assets/Scripts/Objects/Trap.cs
// Автор: parwam
// Код был изменён для нужд проекта

public class Trap : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Ссылка на PlayerController

    private void Start()
    {
        // Проверяем, что ссылка на PlayerController назначена
        if (playerController == null)
        {
            Debug.LogError("PlayerController не назначен! Убедитесь, что вы назначили его в инспекторе.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, что столкновение произошло с объектом, имеющим тег "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController != null)
            {
                playerController.Die(); // Вызываем метод Die()
            }
            else
            {
                Debug.LogError("PlayerController не назначен!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что триггер активирован объектом с тегом "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController != null)
            {
                playerController.Die(); // Вызываем метод Die()
            }
            else
            {
                Debug.LogError("PlayerController не назначен!");
            }
        }
    }
}