using UnityEngine;

// Cho phép tạo file asset mới qua menu chuột phải
[CreateAssetMenu(fileName = "New Gun Data", menuName = "Gun System/Gun Data")]
public class GunData : ScriptableObject
{
    [Header("Shop & Item")]
    public string gunID = "GUN_01";
    public Sprite shopIcon;

    [Header("Pricing")]
    public int priceCoin = 100;
    public int priceGem = 0;

    [Header("Weapon Stats")]
    public float shotDelay = 0.5f; // Thời gian chờ giữa các lần bắn
    public int maxAmmoInClip = 30; // Số đạn tối đa trong băng
    public int maxReserveAmmo = 150; // Số đạn dự trữ tối đa
    public int damage = 10; // Sát thương cơ bản

    [Header("Prefabs")]
    // Trường để kéo Prefab Súng vào
    public GameObject gunPrefab; 
    
    // Trường để kéo Prefab Viên đạn vào
    public GameObject bulletPrefab;
}