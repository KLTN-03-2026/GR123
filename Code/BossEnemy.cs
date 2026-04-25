using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCooldown = 2f ;
    private float nextSkillTime = 0f;
    [SerializeField] private GameObject usbPrefabs; // <-- USB sẽ rơi ra

    // 🔥 Tham chiếu này không cần thiết trong Boss nữa, vì nó không còn gọi WinGame
    // private GameManager gameManager; 
    // private ScoreManager scoreManager; 
    // private bool isWinning = false; 
    
    protected override void Start()
    {
        base.Start();
        // Không cần FindObjectOfType<GameManager>() và ScoreManager() nữa
    }

    protected override void Update()
    {
        base.Update();
        if(Time.time >= nextSkillTime)
        {
            SuDungSkill();
        }
    }

    // ✅ PHẦN ĐÃ SỬA: CHỈ SINH RA USB VÀ GỌI BASE.DIE()
    protected override void Die()
    {
        // 1. Sinh ra USB tại vị trí Boss
        if (usbPrefabs != null)
        {
            // USB phải có Collider 2D và Tag = "USB"
            Instantiate(usbPrefabs, transform.position, Quaternion.identity);
            Debug.Log("Boss bị tiêu diệt. USB (chìa khóa) đã rơi ra.");
        }
        else
        {
            Debug.LogError("usbPrefabs chưa được gán trong BossEnemy!");
        }

        // 2. Gọi Enemy.Die() để thêm điểm (nếu có) và hủy Boss
        base.Die(); 
    }
    
    // ... (Các hàm còn lại giữ nguyên)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Đảm bảo biến 'player' được khởi tạo trong base class
            if (player != null)
                player.TakeDamage(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Đảm bảo biến 'player' được khởi tạo trong base class
            if (player != null)
                player.TakeDamage(stayDamage);
        }
    }
    private void BanDanThuong()
    {
        if (player != null && firePoint != null && bulletPrefabs != null)
        {
            Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet == null)
            {
                enemyBullet = bullet.AddComponent<EnemyBullet>();
            }

            enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
        }
    }

    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                Mathf.Sin(Mathf.Deg2Rad * angle),
                0).normalized;

            GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet == null)
            {
                enemyBullet = bullet.AddComponent<EnemyBullet>();
            }

            enemyBullet.SetMovementDirection(bulletDirection * speedDanVongTron);
        }
    }

    private void HoiMau()
    {
        currentHp = Mathf.Min(currentHp + hpValue, maxHP);
        UpdateHP();
    }

    private void SinhMiniEnemy()
    {
        if (miniEnemy != null)
        {
            Instantiate(miniEnemy, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("miniEnemy chưa được gán trong Inspector!");
        }
    }


    private void DichChuyen()
    {
        if(player !=null)
        {
            transform.position=player.transform.position;
        }
    }
    private void ChonSkillNgauNhien(){
        int randomSkill =Random.Range(0,5);
        switch (randomSkill)
        {
            case 0:
            BanDanThuong();
            break;
            case 1:
            BanDanVongTron();
            break;
            case 2:
            HoiMau();
            break;
            case 3:
            SinhMiniEnemy();
            break;
            case 4:
            DichChuyen();
            break;
        }
    }
    private void SuDungSkill(){
        nextSkillTime = Time.time +skillCooldown;
        ChonSkillNgauNhien();
    }
}