using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LoopingBackground2D : MonoBehaviour
{
    [Tooltip("Скорость прокрутки (единиц в секунду)")]
    public float scrollSpeed = 0.5f;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _initialOffset;
    private Material _material;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material; // Копируем материал
        _initialOffset = _material.mainTextureOffset;
    }

    void Update()
    {
        // Смещаем текстуру по вертикали
        float yOffset = Mathf.Repeat(Time.time * scrollSpeed, 1);
        _material.mainTextureOffset = new Vector2(0, yOffset);
    }

    void OnDisable()
    {
        _material.mainTextureOffset = _initialOffset; // Сброс смещения
    }
}