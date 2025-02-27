using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RH : MonoBehaviour
{
    [SerializeField] private Animator animator;  // Ссылка на Animator
    [SerializeField] private float acceleration = 1f;  // Ускорение
    [SerializeField] private float maxSpeed = 10f;     // Максимальная скорость
    [SerializeField] private TilemapCollider2D levelTilemap;
    [SerializeField] private float SecondBeforeAction; // Время ожидания перед новым действием
    [SerializeField] private bool BlinkDuringIdle;     // Мигать во время простоя
    [SerializeField] private List<RockHeadAnims> selectedActions = new List<RockHeadAnims>();  // Список выбранных анимаций
    [SerializeField] private RotationDirection rotationDirection;  // Направление движения (из Enum)

    private float currentSpeed = 0f;  // Текущая скорость
    private int currentActionIndex = 0; // Номер текущего действия

    private void Start()
    {
        if (selectedActions.Count > 0)
        {
            StartCoroutine(PerformActions());
        }
    }

    private IEnumerator PerformActions()
    {
        while (true)
        {
            // Выполнение действия из списка
            ExecuteAction(selectedActions[currentActionIndex]);

            // Переход к следующему действию
            currentActionIndex = (currentActionIndex + 1) % selectedActions.Count;

            // Ждем заданное время перед следующим действием
            yield return new WaitForSeconds(SecondBeforeAction);
            animator.SetTrigger("Idle");
        }
    }

    private void ExecuteAction(RockHeadAnims action)
    {
        // Сбрасываем все триггеры перед установкой нового
        animator.ResetTrigger("Bottom");
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Right");
        animator.ResetTrigger("Top");

        switch (action)
        {
            case RockHeadAnims.BottomHit:
                animator.SetTrigger("Bottom");
                break;
            case RockHeadAnims.LeftHit:
                animator.SetTrigger("Left");
                break;
            case RockHeadAnims.RightHit:
                animator.SetTrigger("Right");
                break;
            case RockHeadAnims.TopHit:
                animator.SetTrigger("Top");
                break;
            default:
                Debug.LogWarning("Неизвестное действие: " + action);
                break;
        }
    }
}
