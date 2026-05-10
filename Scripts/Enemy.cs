using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField] protected float enemyMoveSpeed = 1f;

    [SerializeField] protected float maxHP = 60f;

    protected float currentHp;

    [SerializeField] protected Image HP;

    [Header("Detection Settings")]
    [SerializeField] protected float detectionRange = 12f;

    [Header("Score Settings")]
    public int scoreValue = 10;

    protected Player player;

    protected bool isPlayerDetected = false;

    private SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();

        currentHp = maxHP;

        UpdateHP();
    }

    protected virtual void Update()
    {
        HandleDetection();

        FlipEnemy();
    }

    private void HandleDetection()
    {
        if (player != null)
        {
            float distanceToPlayer =
                Vector2.Distance(
                    transform.position,
                    player.transform.position
                );

            isPlayerDetected =
                distanceToPlayer <= detectionRange;
        }
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position =
                Vector2.MoveTowards(
                    transform.position,
                    player.transform.position,
                    enemyMoveSpeed * Time.deltaTime
                );
        }
    }

    protected void FlipEnemy()
    {
        if (player == null || spriteRenderer == null) return;

        spriteRenderer.flipX =
            player.transform.position.x < transform.position.x;
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        currentHp -= damage;

        currentHp = Mathf.Max(currentHp, 0);

        UpdateHP();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(
                scoreValue,
                transform.position
            );
        }

        Destroy(gameObject);
    }

    protected void UpdateHP()
    {
        if (HP != null)
        {
            HP.fillAmount = currentHp / maxHP;
        }
    }
}
