using UnityEngine;

public class RotatingTrap : MonoBehaviour
{
    [Header("Настройки вращения")]
    [SerializeField] private float rotationSpeed = 100f; // Скорость вращения
    [SerializeField] private float rotationAngle = 90f;  // Максимальный угол вращения
    [SerializeField] private bool clockwise = true;      // Направление вращения (по часовой стрелке)

    [Header("Настройки центра вращения")]
    [SerializeField] private Transform rotationCenter;   // Центр вращения (можно оставить пустым для автоматического выбора)
    private Vector3 centerOffset;

    private void Start()
    {
        // Если центр вращения не задан, используем позицию TrapPivot
        if (rotationCenter == null)
        {
            rotationCenter = transform;
        }

        // Вычисляем смещение относительно центра вращения
        centerOffset = transform.position - rotationCenter.position;
    }

    private void Update()
    {
        // Вращаем ловушку вокруг центра
        float angle = rotationSpeed * Time.deltaTime;
        if (!clockwise) angle = -angle;

        transform.RotateAround(rotationCenter.position, Vector3.forward, angle);

        // Ограничиваем угол вращения
        float currentAngle = Vector2.SignedAngle(Vector2.right, transform.position - rotationCenter.position);
        if (Mathf.Abs(currentAngle) > rotationAngle)
        {
            rotationSpeed = -rotationSpeed; // Меняем направление вращения
        }
    }
}