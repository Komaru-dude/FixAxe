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
        if (_spriteRenderer != null)
        {
            _material = _spriteRenderer.material; // Копируем материал
            if (_material != null)
                _initialOffset = _material.mainTextureOffset;
        }
    }

    void Update()
    {
        if (_material == null)
            return;

        // Смещаем текстуру по вертикали
        float yOffset = Mathf.Repeat(Time.time * scrollSpeed, 1);
        _material.mainTextureOffset = new Vector2(0, yOffset);
    }

    void OnDisable()
    {
        if (_material != null)
            _material.mainTextureOffset = _initialOffset; // Сброс смещения
    }
}
