using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float deceleration = 30f;
    [SerializeField] private float velocityPower = 0.95f;
    [SerializeField] [Range(0, 1)] private float movementThreshold = 0.3f;
    [SerializeField] private float maxHorizontalSpeed = 12f;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float airJumpForce = 12f;

    [Header("Respawn Settings")]
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private float deathAnimationDuration = 0.5f;
    [SerializeField] private Transform respawnPoint;

    [Header("Visual Settings")]
    [SerializeField] private bool facingRight = true;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer spriteRenderer;
    private float targetSpeed;
    private float speedDifference;
    private bool isMovingLeft;
    private bool isMovingRight;
    private bool isGrounded;
    private bool isDead;
    private float verticalVelocity;
    private int currentJumps;

    public event Action OnJump;
    public event Action<bool> OnGroundedChanged;
    public event Action<float> OnVerticalVelocityChanged;
    public event Action OnDeath;
    public event Action OnRespawn;

    public float MoveSpeed => moveSpeed;
    public float HorizontalSpeed => Mathf.Abs(rb.linearVelocity.x) > movementThreshold ? Mathf.Abs(rb.linearVelocity.x) : 0;
    public bool IsGrounded => isGrounded;
    public bool IsDead => isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        currentJumps = maxJumps;

        if (respawnPoint == null)
            Debug.LogError("Respawn Point not assigned!");
    }

    void Update()
    {
        if (isDead)
        {
            isMovingLeft = false;
            isMovingRight = false;
        }
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        HandleMovement();
        CheckGrounded();
        UpdateVerticalVelocity();
        UpdateFacingDirection();
    }

    private void HandleMovement()
    {
        // Вычисляем целевую скорость (-1, 0, 1) * moveSpeed
        targetSpeed = ((isMovingLeft ? -1f : 0f) + (isMovingRight ? 1f : 0f)) * moveSpeed;

        // Ограничение по максимальной скорости
        if (Mathf.Abs(rb.linearVelocity.x) > maxHorizontalSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxHorizontalSpeed, rb.linearVelocity.y);
            return;
        }

        speedDifference = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > movementThreshold) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelRate, velocityPower) * Mathf.Sign(speedDifference);
        
        rb.AddForce(movement * Vector2.right);
    }

    private void CheckGrounded()
    {
        // Вычисляем точку проверки: нижняя центральная точка коллайдера
        Vector2 rayOrigin = new Vector2(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y);
        bool newGrounded = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);

        if (newGrounded != isGrounded)
        {
            isGrounded = newGrounded;
            if (isGrounded)
                currentJumps = maxJumps;
            OnGroundedChanged?.Invoke(isGrounded);
        }
    }

    private void UpdateVerticalVelocity()
    {
        float currentY = rb.linearVelocity.y;
        if (!Mathf.Approximately(verticalVelocity, currentY))
        {
            verticalVelocity = currentY;
            OnVerticalVelocityChanged?.Invoke(verticalVelocity);
        }
    }

    private void UpdateFacingDirection()
    {
        if (Mathf.Abs(rb.linearVelocity.x) < movementThreshold)
            return;
        
        bool shouldFaceRight = rb.linearVelocity.x > 0;
        if (shouldFaceRight != facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Jump()
    {
        if ((isGrounded || currentJumps > 0) && !isDead)
        {
            // Обнуляем вертикальную скорость для консистентности прыжка
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            float force = isGrounded ? jumpForce : airJumpForce;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            
            currentJumps--;
            OnJump?.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Traps") && !isDead)
            Die();
    }

    public void Die()
    {
        if (isDead || !enabled)
            return;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        isDead = true;
        enabled = false;
        capsuleCollider.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        OnDeath?.Invoke();

        yield return new WaitForSeconds(deathAnimationDuration);
        spriteRenderer.enabled = false;

        float remainingDelay = respawnDelay - deathAnimationDuration;
        if (remainingDelay > 0)
            yield return new WaitForSeconds(remainingDelay);

        Respawn();
        enabled = true;
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.gravityScale = 1;
        capsuleCollider.enabled = true;
        isDead = false;
        spriteRenderer.enabled = true;
        currentJumps = maxJumps;
        OnRespawn?.Invoke();
    }

    public void StartMovingLeft() => isMovingLeft = true;
    public void StopMovingLeft() => isMovingLeft = false;
    public void StartMovingRight() => isMovingRight = true;
    public void StopMovingRight() => isMovingRight = false;

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || capsuleCollider == null)
            return;
        Gizmos.color = Color.red;
        Vector2 rayOrigin = new Vector2(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y);
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * groundCheckDistance);
    }
}
