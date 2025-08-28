using System.Collections;
using UnityEngine;

public class FanTrap : MonoBehaviour
{
    // Направление и сила ветра
    [SerializeField] private Vector2 pushDirection = Vector2.up;
    [SerializeField] private float pushForce = 12f;
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
        if (!isActive)
            return;
        if (!other.CompareTag("Player"))
            return;
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Force);
    }
}