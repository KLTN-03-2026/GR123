using UnityEngine;

public class EnergyEnemy : Enemy
{
    [SerializeField] private GameObject energyObject;

    [Header("Attack Settings")]
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
        if (energyObject != null)
        {
            GameObject energy =
                Instantiate(energyObject, transform.position, Quaternion.identity);

            Destroy(energy, 6f);
        }

        base.Die();
    }
}
