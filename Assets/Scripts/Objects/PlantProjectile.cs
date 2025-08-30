using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private Vector2 direction;

    public void Init(Vector2 dir, float speed, float lifetime)
    {
        this.direction = dir.normalized;
        this.speed = speed;
        this.lifetime = lifetime;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>()?.Die();
            Destroy(gameObject);
        }
    }
}
