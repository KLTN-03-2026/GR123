using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float damageCooldown = 1f;

    private float nextDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (Time.time >= nextDamageTime)
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);

                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }
}