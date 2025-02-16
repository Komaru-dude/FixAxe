using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Animation Parameters")]
    [SerializeField] private string jumpTrigger = "Jump";
    [SerializeField] private string doubleJumpTrigger = "DoubleJump";
    [SerializeField] private string fallTrigger = "Fall";
    [SerializeField] private string groundedParam = "IsGrounded";
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string deathParam = "IsDead";
    [SerializeField] private string respawnTrigger = "Respawn";

    [Header("Animation Settings")]
    [SerializeField] [Range(0.01f, 0.5f)] private float speedSmoothing = 0.05f;
    [SerializeField] private float animationSpeedMultiplier = 1.5f;
    [SerializeField] private float fallVelocityThreshold = -0.1f;

    private Animator animator;
    private PlayerController playerController;
    private float currentSmoothedSpeed;
    private float smoothVelocity;
    private bool isJumping;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        playerController.OnJump += HandleJump;
        playerController.OnGroundedChanged += HandleGroundedChanged;
        playerController.OnVerticalVelocityChanged += HandleVerticalVelocityChanged;
        playerController.OnDeath += HandleDeath;
        playerController.OnRespawn += HandleRespawn;
    }

    void Update()
    {
        float rawSpeed = Mathf.Abs(playerController.HorizontalSpeed);
        float normalizedSpeed = Mathf.Clamp01(rawSpeed / playerController.MoveSpeed);

        if (rawSpeed < 0.01f) normalizedSpeed = 0;

        currentSmoothedSpeed = Mathf.SmoothDamp(
            currentSmoothedSpeed,
            normalizedSpeed,
            ref smoothVelocity,
            speedSmoothing
        );

        float displaySpeed = Mathf.Max(currentSmoothedSpeed, 0.1f) * animationSpeedMultiplier;
        animator.SetFloat(speedParam, displaySpeed);
    }

    private void HandleJump()
    {
        if (playerController.IsGrounded)
        {
            animator.ResetTrigger(jumpTrigger);
            animator.SetTrigger(jumpTrigger);
        }
        else
        {
            animator.ResetTrigger(doubleJumpTrigger);
            animator.SetTrigger(doubleJumpTrigger);
        }
        isJumping = true;
    }

    private void HandleGroundedChanged(bool isGrounded)
    {
        animator.SetBool(groundedParam, isGrounded);
        if (isGrounded) isJumping = false;
    }

    private void HandleVerticalVelocityChanged(float verticalVelocity)
    {
        if (isJumping && verticalVelocity > 0)
            return;

        bool shouldFall = !playerController.IsGrounded &&
                          verticalVelocity < fallVelocityThreshold &&
                          !animator.GetCurrentAnimatorStateInfo(0).IsName("Fall");

        if (shouldFall)
        {
            animator.ResetTrigger(fallTrigger);
            animator.SetTrigger(fallTrigger);
        }
    }

    private void HandleDeath()
    {
        animator.ResetTrigger(fallTrigger);
        animator.ResetTrigger(jumpTrigger);
        animator.ResetTrigger(doubleJumpTrigger);
        animator.SetBool(deathParam, true);
        animator.SetFloat(speedParam, 0);
    }

    private void HandleRespawn()
    {
        animator.ResetTrigger(respawnTrigger);
        animator.SetBool(deathParam, false);
        animator.SetTrigger(respawnTrigger);
    }

    void OnDestroy()
    {
        if (playerController == null) return;

        playerController.OnJump -= HandleJump;
        playerController.OnGroundedChanged -= HandleGroundedChanged;
        playerController.OnVerticalVelocityChanged -= HandleVerticalVelocityChanged;
        playerController.OnDeath -= HandleDeath;
        playerController.OnRespawn -= HandleRespawn;
    }
}