using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPrefab;  // Prefab vụ nổ

    // Hàm tạo vụ nổ
    private void CreateExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Chưa gán explosionPrefab!");
        }
    }

    protected override void Die()
    {
        CreateExplosion();   // Tạo vụ nổ khi chết
        base.Die();          // Gọi Die của Enemy (cộng điểm + destroy enemy)
    }
}
