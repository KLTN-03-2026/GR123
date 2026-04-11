using UnityEngine;

public class BasicEnemy : Enemy
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
                player.TakeDamage(enterDamage);
        }

        if (collision.CompareTag("PlayerBullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                Debug.Log("BasicEnemy hit by bullet, damage: " + bullet.GetDamage());
                TakeDamage(bullet.GetDamage());
                Destroy(bullet.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
                player.TakeDamage(stayDamage);
        }
    }
}
