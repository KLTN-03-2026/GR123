using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 movementDirection;

    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float speed = 5f;

    void Start()
    {
        // Tự hủy viên đạn sau khi hết thời gian tồn tại
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Nếu không có hướng di chuyển thì không thực hiện gì cả
        if (movementDirection == Vector3.zero) return;

        // Di chuyển viên đạn theo hướng đã chỉ định
        transform.position += movementDirection * speed * Time.deltaTime;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction.normalized;  // Bình thường hóa hướng di chuyển để đảm bảo tốc độ nhất quán
    }
}
