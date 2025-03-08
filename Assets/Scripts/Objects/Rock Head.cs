using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RockHead : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private TilemapCollider2D levelTilemap;
    [SerializeField] private float SecondBeforeAction = 1f;
    [SerializeField] private List<RockHeadAnims> selectedActions = new List<RockHeadAnims>();
    [SerializeField] private LayerMask layerMask;

    private Collider2D selfCollider;
    private Rigidbody2D rb;
    private Animator animator;
    private int currentActionIndex = 0;
    public Vector2 direction = Vector2.up;
    public float distance = 0f;

    private void Start()
    {
        selfCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (selectedActions.Count > 0)
        {
            StartCoroutine(PerformActions());
        }
        else
        {
            Debug.Log("Нет действий для выполнения");
        }
    }

    private IEnumerator PerformActions()
    {
        while (true)
        {
            RockHeadAnims currentAction = selectedActions[currentActionIndex];
            yield return StartCoroutine(ExecuteAction(currentAction));

            currentActionIndex = (currentActionIndex + 1) % selectedActions.Count;
            yield return new WaitForSeconds(SecondBeforeAction);
            animator.SetTrigger("Idle");
        }
    }

    private IEnumerator ExecuteAction(RockHeadAnims action)
    {
        // Сброс всех триггеров анимации
        animator.ResetTrigger("Bottom");
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Right");
        animator.ResetTrigger("Top");
        animator.ResetTrigger("Blink");

        // Вычисляем начальную точку для Raycast
        Vector2 rayOrigin;
        RaycastHit2D hit;

        // В зависимости от направления и действия запускаем соответствующую анимацию
        switch (action)
        {
            case RockHeadAnims.TopHit:
                animator.SetTrigger("Top");
                direction = Vector2.up;
                rayOrigin = (Vector2)transform.position + direction * selfCollider.bounds.extents.y;
                break;

            case RockHeadAnims.LeftHit:
                animator.SetTrigger("Left");
                direction = Vector2.left;
                rayOrigin = (Vector2)transform.position + direction * selfCollider.bounds.extents.x;
                break;

            case RockHeadAnims.RightHit:
                animator.SetTrigger("Right");
                direction = Vector2.right;
                rayOrigin = (Vector2)transform.position + direction * selfCollider.bounds.extents.x;
                break;

            case RockHeadAnims.BottomHit:
                animator.SetTrigger("Bottom");
                direction = Vector2.down;
                rayOrigin = (Vector2)transform.position + direction * selfCollider.bounds.extents.y;
                break;

            default:
                yield break;
        }

        // Запускаем Raycast один раз после выбора направления
        hit = Physics2D.Raycast(rayOrigin, direction, Mathf.Infinity, layerMask);
        Debug.DrawRay(rayOrigin, direction * distance, Color.red, 2f);

        if (hit.collider != null)
        {
            distance = hit.distance;
        }
        else
        {
            Debug.LogWarning($"Не удалось проверить столкновение с коллайдером ({action})");
            distance = 0f; // Чтобы избежать зацикливания
        }
        // Выполнение движения
        yield return MoveInDirection(direction, distance);
    }

    private IEnumerator MoveInDirection(Vector2 direction, float distance)
    {
        float currentSpeed = 0f;
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = startPosition + direction * distance;

        while (Vector2.Distance(rb.position, targetPosition) > 0.01f)
        {
            // Увеличиваем скорость с ускорением
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            // Запускаем анимацию
            animator.SetTrigger("Blink");

            // Плавное перемещение к цели с учётом текущей скорости
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, currentSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

            yield return null;
        }
    }
}
