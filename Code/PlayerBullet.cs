using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 26f;
    [SerializeField] private float timeDestroy = 0.6f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject bloodPrefabs;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D is missing on bullet. Adding one.");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;

        // Dùng velocity thay vì linearVelocity (chuẩn hơn)
        rb.linearVelocity = transform.right * moveSpeed;

        Destroy(gameObject, timeDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ❌ Bỏ qua va chạm với Player (đạn không bị hủy ngay khi bắn)
        if (collision.CompareTag("Player"))
            return;

        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        // Nếu trúng Enemy thì gây damage + hiệu ứng máu
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.Log("Bullet hit " + collision.gameObject.name + ", dealing " + damage + " damage.");
                enemy.TakeDamage(damage);
                if (bloodPrefabs != null)
                {
                    GameObject blood = Instantiate(bloodPrefabs, transform.position, Quaternion.identity);
                    Destroy(blood, 1f);
                }
            }
        }

        // Trúng bất kỳ vật gì khác thì hủy đạn
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}
