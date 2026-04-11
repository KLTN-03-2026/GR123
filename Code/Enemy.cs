using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Cần thiết cho Coroutine

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField] protected float enemyMoveSpeed = 1f;
    [SerializeField] protected float maxHP = 60f;
    protected float currentHp;

    [SerializeField] protected Image HP;
    [SerializeField] protected float enterDamage = 10f; // Sát thương va chạm ban đầu
    [SerializeField] protected float stayDamage = 1f;   // Sát thương liên tục mỗi lần

    [Header("DOT Settings")]
    [SerializeField] private float damageRate = 3f; // 🔥 ĐÃ CHỈNH SỬA: Giá trị mới là 3 giây
    private Coroutine damageCoroutine; 

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 12f; 
    private bool isPlayerDetected = false; 

    [Header("Score Settings")]
    public int scoreValue = 10; 

    protected Player player;
    private bool isPlayerInRange = false; 

    protected virtual void Start()
    {
        // Tối ưu: Tìm Player chỉ một lần
        // Nếu Player không phải là Singleton, đây là cách làm hợp lý
        player = FindObjectOfType<Player>();
        
        // Khởi tạo máu và UI
        currentHp = maxHP;
        UpdateHP();

        // Kiểm tra Coroutine để đảm bảo không bị lỗi nếu script được kích hoạt lại
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    protected virtual void Update()
    {
        // 1. Xử lý Logic Phát hiện Player (giữ nguyên)
        HandleDetection();

        // 2. Di chuyển CHỈ KHI Player được phát hiện (giữ nguyên)
        if (isPlayerDetected)
        {
            MoveToPlayer();
        }
    }

    // --- LOGIC PHÁT HIỆN & DI CHUYỂN ---

    private void HandleDetection()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                isPlayerDetected = true;
            }
            else if (isPlayerDetected)
            {
                isPlayerDetected = false;
            }
        }
        else
        {
            // Tối ưu: Nếu Player bị phá hủy, dừng đuổi theo
            isPlayerDetected = false;
        }
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                enemyMoveSpeed * Time.deltaTime
            );
        }
    }

    // --- LOGIC SÁT THƯƠNG & VA CHẠM ---

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player playerComponent = collision.GetComponent<Player>();

            if (playerComponent != null)
            {
                // Gây sát thương khởi đầu (enterDamage)
                playerComponent.TakeDamage(enterDamage);
                
                // Đánh dấu Player đã ở trong phạm vi va chạm
                isPlayerInRange = true;
                
                // Bắt đầu Coroutine sát thương liên tục
                if (damageCoroutine == null)
                {
                    // Truyền Player component vào Coroutine để đảm bảo tham chiếu hợp lệ
                    damageCoroutine = StartCoroutine(DamageOverTimeCoroutine(playerComponent));
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            // Dừng Coroutine khi Player thoát khỏi vùng va chạm
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
    
    private IEnumerator DamageOverTimeCoroutine(Player target)
    {
        // Lặp khi player còn ở trong phạm vi VÀ player vẫn còn tồn tại
        while (isPlayerInRange && target != null) 
        {
            // Gây sát thương (stayDamage)
            target.TakeDamage(stayDamage);
            
            // Chờ theo tần suất đã đặt (3 giây)
            yield return new WaitForSeconds(damageRate); 
        }
        
        damageCoroutine = null; 
    }

    // --- LOGIC MÁU & CHẾT ---

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
        Debug.Log("Enemy died.");

        // Dừng Coroutine khi kẻ địch bị tiêu diệt để tránh lỗi tham chiếu
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue, transform.position);
        }
        else
        {
            Debug.LogError("LỖI CỘNG ĐIỂM: ScoreManager.instance KHÔNG ĐƯỢC TÌM THẤY.");
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