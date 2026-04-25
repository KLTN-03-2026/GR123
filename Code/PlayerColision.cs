using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Đảm bảo gán trong Inspector: GameManager, Audio.
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Audio audio; 
    [SerializeField] private float bulletDamage = 10f;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>(); 
        if (player == null)
        {
            Debug.LogWarning("Player component not found in parent!");
        }

        // Khuyến nghị: Đảm bảo các tham chiếu quan trọng đã được gán.
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing in PlayerCollision!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Đạn Enemy bắn trúng player
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("BossBullet"))
        {
            if (player != null)
                player.TakeDamage(bulletDamage);
            
            // Đảm bảo hủy đạn sau khi va chạm
            Destroy(collision.gameObject);
        }

        // 2. Player lượm ENERGY
        else if (collision.CompareTag("Energy"))
        {
            if (gameManager != null)
                gameManager.AddEnergy();
            
            if (audio != null)
                audio.PlayEnergySound();
            
            Destroy(collision.gameObject);
        }

        // 3. USB thắng game (KÍCH HOẠT WIN GAME KHI NHẶT KHÓA/USB)
        else if (collision.CompareTag("USB"))
        {
            if (gameManager != null)
            {
                // Lấy điểm số hiện tại (Dùng Singleton Instance là cách tốt nhất)
                int finalScore = ScoreManager.instance != null
                    ? ScoreManager.instance.GetCurrentScore()
                    : 0;

                // Gọi hàm WinGameLevel, chỉ cần truyền finalScore.
                gameManager.WinGameLevel(finalScore); 
            }

            Destroy(collision.gameObject);
        }
    }
}