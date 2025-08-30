using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float delay = 2f;
    private Animator animator;
    private bool isActive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator не назначен!");
    }

    private void Start()
    {
        StartCoroutine(ToggleBoolCoroutine());
    }

    private IEnumerator ToggleBoolCoroutine()
    {
        while (true)
        {
            isActive = !isActive;
            if (animator != null)
                animator.SetBool("On", isActive);

            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !isActive) 
            return;

        playerController?.Die();

        if (playerController == null)
            Debug.LogError("PlayerController не назначен!");
    }
}
