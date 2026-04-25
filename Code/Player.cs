using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHP = 100f;
    private float currentHp;
    [SerializeField] private Image HP;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentHp = maxHP;
        UpdateHP();
    }

    void Update()
    {
        MovePlayer();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGameMenu();
        }
    }

    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        if (playerInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (animator != null)
        {
            animator.SetBool("IsRun", playerInput != Vector2.zero);
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        Debug.Log("Player took damage: " + damage + ", Current Health: " + currentHp);

        UpdateHP();

        if (currentHp <= 0)
        {
            Die();
        }
        else if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }
    public void Heal(float HealValue)
    {
        if(currentHp < maxHP)
        {
            currentHp+= HealValue;
            currentHp=Mathf.Min(currentHp,maxHP);
            UpdateHP();
            
        }
    }
    public void Die()
    {
        gameManager.GameOverMenu();
    }

    private void UpdateHP()
    {
        if (HP != null)
        {
            HP.fillAmount = currentHp / maxHP;
        }
    }
}
