using UnityEngine;
using UnityEngine.SceneManagement;

public class TrophyController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    
    [Header("Components")]
    [SerializeField] private Animator trophyAnimator;
    [SerializeField] private ParticleSystem confettiParticles;
    [SerializeField] private Collider2D trophyCollider;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Canvas LevelCanvas;
    [SerializeField] private Canvas WinCanvas;
    
    private bool isActivated = false;
    private static readonly int ActivateTrigger = Animator.StringToHash("Activate");

    private void Start()
    {
        // Автоматическое получение ссылок если не установлены
        if (trophyAnimator == null)
            trophyAnimator = GetComponent<Animator>();
        
        if (confettiParticles == null)
            confettiParticles = GetComponentInChildren<ParticleSystem>();
        
        if (trophyCollider == null)
            trophyCollider = GetComponent<Collider2D>();

        // Проверки компонентов
        if (trophyCollider == null)
            Debug.LogError("Missing Collider2D component!", this);
        
        if (trophyCollider != null && !trophyCollider.isTrigger)
            Debug.LogWarning("Collider should be marked as Trigger!", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Trophy] Collision detected with: {other.name}");
        
        if (!isActivated && other.CompareTag(playerTag))
        {
            Debug.Log("[Trophy] Valid player collision!");
            ActivateTrophy();
        }
    }

    private void ActivateTrophy()
    {
        isActivated = true;
        Debug.Log("[Trophy] Activation started");

        // Анимация
        if (trophyAnimator != null)
        {
            trophyAnimator.SetTrigger(ActivateTrigger);
            Debug.Log("[Trophy] Animation triggered");
        }
        else
        {
            Debug.LogWarning("[Trophy] Missing Animator reference!");
        }

        // Конфетти
        if (confettiParticles != null)
        {
            confettiParticles.Play();
            Debug.Log("[Trophy] Confetti played");
        }
        else
        {
            Debug.LogWarning("[Trophy] Missing ParticleSystem reference!");
        }

        // Сохранение прогресса
        SaveLevelProgress();

        // Отключаем коллайдер
        if (trophyCollider != null)
        {
            trophyCollider.enabled = false;
            Debug.Log("[Trophy] Collider disabled");
        }

        // Загрузка LevelSelector
        Invoke(nameof(LoadWinCanvas), 1f); // Просто заменили yield на Invoke
        Debug.Log("[Trophy] Level selector load scheduled");
    }

    private void SaveLevelProgress()
    {
        levelManager.CompleteLevel();
        Debug.Log($"[Trophy] Progress saved. ");
    }

    private void LoadWinCanvas()
    {
        Debug.Log("[Trophy] Loading win canvas...");
        LevelCanvas.gameObject.SetActive(false);
        WinCanvas.gameObject.SetActive(true);
    }
}