using UnityEditor;
using UnityEngine;

// Оригинальный код взят отсюда: https://github.com/parwam/Pixel-Adventure/blob/main/Assets/Scripts/Objects/Pendulum.cs
// Автор: parwam
// Код был изменён для нужд проекта

public class Pendulum : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private Rigidbody2D rb2d; // Ссылка на Rigidbody2D объекта
    [SerializeField] private Transform plank;  // Точка опоры маятника
    [SerializeField] private Transform ball;   // Конец маятника (шар)

    [Header("Параметры движения")]
    [SerializeField] private bool closedLoop = true; // Замкнутый цикл (полный круг или дуга)
    [SerializeField] private RotationDirection rotationDirection = RotationDirection.clockwise; // Направление вращения
    [SerializeField] private float moveSpeed;  // Скорость движения
    [SerializeField] private float leftAngle;  // Левый угол ограничения (для незамкнутого цикла)
    [SerializeField] private float rightAngle; // Правый угол ограничения (для незамкнутого цикла)
    [SerializeField] private Quaternion defaultRot = Quaternion.Euler(0, 0, 0); // Начальный поворот

    private bool movingClockwise = true; // Флаг направления движения (по часовой стрелке)
    private float direction = -1;        // Направление: -1 для по часовой стрелке, 1 против часовой стрелки

    private float angle;                 // Текущий угол поворота
    private float radius;                // Радиус маятника

    private Vector3 initPos;             // Начальная позиция объекта

    void Awake()
    {
        // Если Rigidbody2D не назначен через инспектор, попробуем получить его автоматически
        if (rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Инициализация начальной позиции и угла поворота
        initPos = transform.position;
        angle = GetObjectRotation();

        // Установка начального направления вращения
        switch (rotationDirection)
        {
            case RotationDirection.clockwise:
                movingClockwise = true;
                direction = -1;
                break;
            case RotationDirection.anticlockwise:
                movingClockwise = false;
                direction = 1;
                break;
            default:
                movingClockwise = true;
                direction = -1;
                break;
        }
    }

    // Получение текущего угла поворота объекта
    private float GetObjectRotation()
    {
        if (transform.eulerAngles.z > 180)
        {
            return transform.eulerAngles.z - 360;
        }
        else
        {
            return transform.eulerAngles.z;
        }
    }

    void Update()
    {
        Move(); // Вызов метода движения
    }

    // Изменение направления движения
    private void ChangeDir()
    {
        movingClockwise = !movingClockwise;
        direction *= -1;
    }

    // Метод движения маятника
    private void Move()
    {
        angle += moveSpeed * direction * Time.deltaTime;

        // Ограничение углов для незамкнутого цикла
        if (!closedLoop)
        {
            if (movingClockwise)
            {
                if (angle < leftAngle)
                {
                    angle = leftAngle;
                    ChangeDir();
                }
            }
            else
            {
                if (angle > rightAngle)
                {
                    angle = rightAngle;
                    ChangeDir();
                }
            }
        }

        // Применение поворота к Rigidbody2D
        rb2d.MoveRotation(angle);
    }

    // Рисование вспомогательных линий в редакторе Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        // Проверка, что plank и ball назначены перед использованием их позиций
        if (plank == null || ball == null)
        {
            return; // Выход из метода, если ссылки не назначены
        }

        radius = Vector3.Distance(plank.position, ball.position);

#if UNITY_EDITOR
        // Определение центра в зависимости от состояния игры
        Vector3 center = EditorApplication.isPlaying ? initPos : transform.position;
#else
        Vector3 center = transform.position;
#endif

        // Рисование круга или дуги в зависимости от режима
        if (closedLoop)
        {
            DrawCircle(center, radius); // Рисование полного круга
        }
        else
        {
            float arcStart = leftAngle;
            float arcEnd = rightAngle;
            DrawArc(center, defaultRot, radius, arcStart, arcEnd, 0.1f); // Рисование дуги
        }
    }

    // Рисование полного круга
    private void DrawCircle(Vector3 center, float r)
    {
        float theta = 0;
        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);
        Vector3 previous = center + new Vector3(x, y, 0);

        for (int i = 1; i <= 360; i++)
        {
            theta = Mathf.Deg2Rad * i;
            x = r * Mathf.Cos(theta);
            y = r * Mathf.Sin(theta);
            Vector3 next = center + new Vector3(x, y, 0);
            Gizmos.DrawLine(previous, next);
            previous = next;
        }
    }

    // Рисование дуги
    private void DrawArc(Vector3 center, Quaternion rot, float r, float startAngle, float endAngle, float step)
    {
        Quaternion pendulumRotation = Quaternion.Euler(0, 0, rot.eulerAngles.z);
        Vector3 startPoint = Quaternion.Euler(0, 0, startAngle) * Vector3.down * r;
        startPoint = center + pendulumRotation * startPoint;
        Vector3 previous = startPoint;

        for (float theta = startAngle + step; theta <= endAngle; theta += step)
        {
            Vector3 nextPoint = Quaternion.Euler(0, 0, theta) * Vector3.down * r;
            nextPoint = center + pendulumRotation * nextPoint;
            Gizmos.DrawLine(previous, nextPoint);
            previous = nextPoint;
        }
    }
}