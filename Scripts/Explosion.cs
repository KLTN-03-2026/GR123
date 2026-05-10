
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private bool hasDamagedPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage player 1 lần
        if (collision.CompareTag("Player") && !hasDamagedPlayer)
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);

                hasDamagedPlayer = true;

                Debug.Log("Player bị nổ: " + damage);
            }
        }

        // Damage enemy
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
