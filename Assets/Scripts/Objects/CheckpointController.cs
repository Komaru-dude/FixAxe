using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint; // Сюда передаём респавн-пойнт
    public float xOffset = 2f; // Насколько передвинуть респавн-пойнт
    private Animator animator;
    private bool isTriggered;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            // Перемещаем респавн-пойнт левее/правее чекпоинта
            respawnPoint.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
            isTriggered = true;
            
            // Запускаем анимацию чекпоинта
            if (animator != null)
            {
                animator.SetTrigger("Claim");
            }
        }
    }
}
