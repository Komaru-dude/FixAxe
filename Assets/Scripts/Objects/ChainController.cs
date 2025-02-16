using UnityEngine;

public class ChainController : MonoBehaviour
{
    [Header("Chain Settings")]
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform ball;
    [SerializeField][Range(2, 50)] private int segments = 10;
    [SerializeField] private float textureTiling = 1f;
    [SerializeField] private float lineWidth = 0.1f;

    private LineRenderer lineRenderer;
    private Vector3[] chainPositions;

    void Start()
    {
        InitializeChain();
        ApplyTextureSettings();
    }

    void InitializeChain()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments;
        chainPositions = new Vector3[segments];
        
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.textureMode = LineTextureMode.Tile;
    }

    void ApplyTextureSettings()
    {
        // Перенесите эти настройки в материал в редакторе:
        // 1. Выберите материал цепи в проекте
        // 2. В инспекторе: Texture Wrap Mode = Repeat
        // 3. Настройте Tiling через параметр textureTiling в скрипте
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(textureTiling, 1);
    }

    void Update()
    {
        if (pivot == null || ball == null) return;
        
        UpdateChainPositions();
        UpdateTextureTiling();
        FixTransformIssues();
    }

    void FixTransformIssues()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    void UpdateChainPositions()
    {
        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);
            chainPositions[i] = Vector3.Lerp(pivot.position, ball.position, t);
        }
        lineRenderer.SetPositions(chainPositions);
    }

    void UpdateTextureTiling()
    {
        float distance = Vector3.Distance(pivot.position, ball.position);
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(
            distance * textureTiling, 
            1
        );
    }
}