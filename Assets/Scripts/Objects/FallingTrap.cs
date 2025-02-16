using UnityEngine;

// Оригинальный код взят отсюда: https://github.com/parwam/Pixel-Adventure/blob/main/Assets/Scripts/Objects/Trap.cs
// Автор: parwam
// Код был изменён для нужд проекта

public class FTrap : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Ссылка на PlayerController
    [SerializeField] private Animator animator;                // Ссылка на Animator для анимации
    [SerializeField] private float acceleration = 5f;            // Ускорение падения
    [SerializeField] private float totalFallTime = 3f;           // Общее время падения
    [SerializeField] private float fadeDuration = 1f;            // Длительность затухания (в конце падения)
    [SerializeField] public float randomMoveInterval = 1f;  // Интервал между сдвигами
    [SerializeField] public float randomMoveDistance = 0.25f;  // Расстояние для движения в одну сторону
    [SerializeField] public float randomMoveSpeed = 0.25f;     // Скорость движения

    private Vector3 startPosition;   // Начальная позиция объекта
    private Vector3 targetPosition;  // Целевая позиция объекта
    private bool isMovingForward = true; // Флаг, указывающий на направление движения

    private bool activated = false;
    private float fallTimer = 0f;
    private float currentFallSpeed = 0f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Проверка ссылки на PlayerController
        if (playerController == null)
        {
            Debug.LogError("PlayerController не назначен! Убедитесь, что вы назначили его в инспекторе.");
        }

        // Получаем SpriteRenderer для управления прозрачностью
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на объекте ловушки!");
        }
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(randomMoveDistance, 0, 0);
    }

    private void Update()
    {
        if (activated)
        {
            // Обновляем таймер падения
            fallTimer += Time.deltaTime;

            // Плавное увеличение скорости
            currentFallSpeed += acceleration * Time.deltaTime;
            transform.Translate(Vector3.down * currentFallSpeed * Time.deltaTime);

            // Начинаем затухание за fadeDuration до конца падения
            if (fallTimer > (totalFallTime - fadeDuration) && spriteRenderer != null)
            {
                float fadeProgress = (fallTimer - (totalFallTime - fadeDuration)) / fadeDuration; // 0 до 1
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(1f, 0f, fadeProgress);
                spriteRenderer.color = color;
            }

            // По окончании падения удаляем объект
            if (fallTimer >= totalFallTime)
            {
                Destroy(gameObject);
            }
        }
        if (activated) return;  // Если объект активирован, движение не происходит

        // Плавное движение объекта по времени
        MoveBackAndForth();
    }

    // Метод активации ловушки
    private void ActivateTrap()
    {
        if (!activated)
        {
            activated = true;
            // Запускаем анимацию, если Animator назначен
            if (animator != null)
                animator.SetTrigger("On");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivateTrap();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivateTrap();
        }
    }
    void MoveBackAndForth()
    {
        // Сдвигаем объект в сторону целевой позиции
        if (isMovingForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, randomMoveSpeed * Time.deltaTime);

            // Если объект достиг целевой позиции, меняем направление
            if (transform.position == targetPosition)
            {
                isMovingForward = false;
                // Задаем новую целевую позицию для обратного движения
                targetPosition = startPosition;
            }
        }
        else
        {
            // Сдвигаем объект обратно в исходную позицию
            transform.position = Vector3.MoveTowards(transform.position, startPosition, randomMoveSpeed * Time.deltaTime);

            // Если объект вернулся на начальную позицию, меняем направление
            if (transform.position == startPosition)
            {
                isMovingForward = true;
                // Задаем новую целевую позицию для движения вперед
                targetPosition = startPosition + new Vector3(randomMoveDistance, 0, 0);
            }
        }
    }

    // Метод для активации/деактивации
    public void ToggleActivation(bool activate)
    {
        activated = activate;
    }
}
