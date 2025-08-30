using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float attackRate = 5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Настройки проджектайла")]
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileLifetime = 3f;

    private float attackTimer;
    private Animator animator;
    private bool isDead = false;
    private bool isAttacking = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        attackTimer = attackRate;
    }

    void Update()
    {
        if (isDead || isAttacking) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackRate;
        animator.SetTrigger("Attack");
    }

    public void Shoot()
    {
        if (isDead) return;

        Vector2 dir = Vector2.left;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(dir, projectileSpeed, projectileLifetime);
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        animator.Play("Plant_idle", 0, 0f);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
    }

    public void OnDieAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }
}