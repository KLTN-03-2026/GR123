using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private float healValue = 20f;    // Số lượng máu hồi lại cho người chơi khi kẻ thù chết

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi va chạm với người chơi
        if (collision.CompareTag("Player"))
        {
                player.TakeDamage(enterDamage);  // Gây sát thương khi va chạm
        }

        // Khi va chạm với viên đạn của người chơi
        if (collision.CompareTag("PlayerBullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                Debug.Log("Kẻ thù bị trúng đạn, sát thương: " + bullet.GetDamage());
                TakeDamage(bullet.GetDamage());  // Kẻ thù bị sát thương bởi viên đạn
                Destroy(bullet.gameObject);  // Hủy viên đạn
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Khi người chơi đứng trong phạm vi của kẻ thù, gây sát thương liên tục
        if (collision.CompareTag("Player"))
        {
                player.TakeDamage(stayDamage * Time.deltaTime);  // Sát thương theo thời gian
 
        }
    }

    // Xử lý khi kẻ thù chết
    protected override void Die()
    {
        HealPlayer();  // Hồi máu cho người chơi khi kẻ thù chết

        base.Die();  // Gọi phương thức Die của lớp cha để xử lý cái chết của kẻ thù
    }

    // Hồi máu cho người chơi
    private void HealPlayer()
    {
        // Tìm người chơi trong scene
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            player.Heal(healValue);  // Hồi máu cho người chơi
        }
        else
        {
            Debug.LogWarning("Không tìm thấy người chơi trong scene!");  // Cảnh báo nếu không tìm thấy người chơi
        }
    }
}
