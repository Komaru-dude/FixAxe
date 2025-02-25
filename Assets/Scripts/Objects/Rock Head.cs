using System.Collections.Generic;
using UnityEngine;

public class RH : MonoBehaviour
{
    [SerializeField] private Animator animator;  // Ссылка на Animator
    [SerializeField] private float acceleration = 1f;  // Ускорение
    [SerializeField] private float maxSpeed = 10f;     // Максимальная скорость
    [SerializeField] private LayerMask wallLayer;      // Слой стены
    [SerializeField] private List<RockHeadAnims> selectedActions = new List<RockHeadAnims>();  // Список выбранных анимаций
    [SerializeField] private RotationDirection rotationDirection;  // Направление движения (из Enum)

    private float currentSpeed = 0f;  // Текущая скорость

    void Update()
    {

    }
}
