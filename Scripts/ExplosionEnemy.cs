
using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private float attackRange = 2f;

    private Animator animator;

    private bool hasExploded = false;

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
            Vector2.Distance(transform.position, player.transform.position);

        bool isAttack = distance <= attackRange;

        animator.SetBool("IsAttack", isAttack);

        if (!isAttack && isPlayerDetected)
        {
            MoveToPlayer();
        }

        // Chạm player -> nổ
        if (distance <= 1f && !hasExploded)
        {
            ExplodeNow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bị đạn bắn
        if (collision.CompareTag("PlayerBullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                TakeDamage(bullet.GetDamage());

                Destroy(bullet.gameObject);
            }
        }
    }

    public void ExplodeNow()
    {
        if (hasExploded) return;

        hasExploded = true;

        CreateExplosion();

        Destroy(gameObject);
    }

    private void CreateExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }

    protected override void Die()
    {
        // Chết do đạn -> nổ
        ExplodeNow();

        base.Die();
    }
}
