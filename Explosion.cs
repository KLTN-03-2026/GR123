using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 25f;  // Sát thương của vụ nổ

    // Hàm xử lý khi có va chạm
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem va chạm với đối tượng "Player"
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);  // Gây sát thương cho người chơi
                Debug.Log("Người chơi bị nổ, sát thương: " + damage);
            }
        }

        // Kiểm tra xem va chạm với đối tượng "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);  // Gây sát thương cho kẻ thù
                Debug.Log("Kẻ thù bị nổ, sát thương: " + damage);
            }
        }
    }

    // Hàm hủy đối tượng vụ nổ
    public void DestroyExplosion()
    {
        Destroy(gameObject);  // Xóa đối tượng vụ nổ khỏi scene
    }
}
