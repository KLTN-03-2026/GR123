
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;

    [SerializeField] private float hpValue = 100f;

    [SerializeField] private GameObject miniEnemy;

    [SerializeField] private float skillCooldown = 2f;
    private float nextSkillTime = 0f;

    [SerializeField] private GameObject usbPrefabs;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 4f;

    private Animator animator;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.transform.position
            );

        bool isAttack = distance <= attackRange;

        animator.SetBool("IsAttack", isAttack);

        // Run
        if (!isAttack && isPlayerDetected)
        {
            MoveToPlayer();
        }

        // Attack
        if (isAttack && Time.time >= nextSkillTime)
        {
            SuDungSkill();
        }
    }

    protected override void Die()
    {
        if (usbPrefabs != null)
        {
            Instantiate(
                usbPrefabs,
                transform.position,
                Quaternion.identity
            );

            Debug.Log("Boss bị tiêu diệt. USB đã rơi ra.");
        }

        base.Die();
    }

    private void BanDanThuong()
    {
        if (player != null &&
            firePoint != null &&
            bulletPrefabs != null)
        {
            Vector3 directionToPlayer =
                (
                    player.transform.position -
                    firePoint.position
                ).normalized;

            GameObject bullet =
                Instantiate(
                    bulletPrefabs,
                    firePoint.position,
                    Quaternion.identity
                );

            EnemyBullet enemyBullet =
                bullet.GetComponent<EnemyBullet>();

            if (enemyBullet == null)
            {
                enemyBullet =
                    bullet.AddComponent<EnemyBullet>();
            }

            enemyBullet.SetMovementDirection(
                directionToPlayer * speedDanThuong
            );
        }
    }

    private void BanDanVongTron()
    {
        const int bulletCount = 12;

        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            Vector3 bulletDirection =
                new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * angle),
                    Mathf.Sin(Mathf.Deg2Rad * angle),
                    0
                ).normalized;

            GameObject bullet =
                Instantiate(
                    bulletPrefabs,
                    transform.position,
                    Quaternion.identity
                );

            EnemyBullet enemyBullet =
                bullet.GetComponent<EnemyBullet>();

            if (enemyBullet == null)
            {
                enemyBullet =
                    bullet.AddComponent<EnemyBullet>();
            }

            enemyBullet.SetMovementDirection(
                bulletDirection * speedDanVongTron
            );
        }
    }

    private void HoiMau()
    {
        currentHp =
            Mathf.Min(currentHp + hpValue, maxHP);

        UpdateHP();
    }

    private void SinhMiniEnemy()
    {
        if (miniEnemy != null)
        {
            Instantiate(
                miniEnemy,
                transform.position,
                Quaternion.identity
            );
        }
    }

    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position =
                player.transform.position +
                new Vector3(1.5f, 0, 0);
        }
    }

    private void ChonSkillNgauNhien()
    {
        int randomSkill = Random.Range(0, 5);

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

    private void SuDungSkill()
    {
        nextSkillTime =
            Time.time + skillCooldown;

        ChonSkillNgauNhien();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            PlayerBullet bullet =
                collision.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                TakeDamage(
                    bullet.GetDamage()
                );

                Destroy(bullet.gameObject);
            }
        }
    }
}
