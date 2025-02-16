using UnityEngine;

public class TrapTilemap : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Ссылка на PlayerController

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController не назначен! Убедитесь, что вы указали его в инспекторе.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerController != null)
        {
            playerController.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerController != null)
        {
            playerController.Die();
        }
    }
}
