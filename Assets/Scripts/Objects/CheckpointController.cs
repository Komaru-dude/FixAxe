using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint; // Сюда передаём респавн-пойнт
    public float yOffset = 2f; // Насколько поднять респавн-пойнт
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // Берём аниматор чекпоинта
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Проверяем, что это игрок
        {
            // Перемещаем респавн-пойнт вверх
            respawnPoint.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y + yOffset, respawnPoint.position.z);
            
            // Запускаем анимацию чекпоинта
            if (animator != null)
            {
                animator.SetTrigger("Activate"); // В аниматоре должен быть триггер "Activate"
            }
        }
    }
}
