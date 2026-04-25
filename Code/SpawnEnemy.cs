using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;  // Loại enemy
    public Transform spawnPoint;     // Vị trí spawn
}

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private EnemySpawnData[] spawnData;   // Mảng chứa dữ liệu spawn
    [SerializeField] private float timeBetweenSpawns = 2f; // Thời gian chờ trước khi spawn

    private void Start()
    {
        // Đảm bảo có dữ liệu spawn và thời gian chờ hợp lệ
        if (spawnData.Length > 0 && timeBetweenSpawns > 0)
        {
            StartCoroutine(SpawnAllEnemiesOnceCoroutine());
        }
        else
        {
            Debug.LogWarning("SpawnEnemy: Không có dữ liệu spawn hoặc thời gian spawn không hợp lệ.");
        }
    }

    private IEnumerator SpawnAllEnemiesOnceCoroutine()
    {
        // 1. Chờ một khoảng thời gian (VD: 2 giây)
        yield return new WaitForSeconds(timeBetweenSpawns);

        // 2. Duyệt qua TẤT CẢ các phần tử trong mảng spawnData
        foreach (var data in spawnData)
        {
            // 3. Tiến hành spawn cho từng phần tử
            if (data.enemyPrefab != null && data.spawnPoint != null)
            {
                // Instantiate enemy tại vị trí spawn point tương ứng
                Instantiate(data.enemyPrefab, data.spawnPoint.position, Quaternion.identity);
            }
        }
        
        // Coroutine kết thúc sau khi spawn xong tất cả các enemy.
    }
}