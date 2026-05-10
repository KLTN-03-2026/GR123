
using UnityEngine;

public class HealEnemy : Enemy
{
    [SerializeField] private float healValue = 20f;

    [SerializeField] private float attackRange = 2f;

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
            Vector2.Distance(transform.position, player.transform.position);

        bool isAttack = distance <= attackRange;

        animator.SetBool("IsAttack", isAttack);

        if (!isAttack && isPlayerDetected)
        {
            MoveToPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

    protected override void Die()
    {
        HealPlayer();

        base.Die();
    }

    private void HealPlayer()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            player.Heal(healValue);
        }
    }
}
