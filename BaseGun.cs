using UnityEngine;

public class BaseGun : MonoBehaviour
{
    // Lưu trữ dữ liệu chỉ số súng (Được truyền từ PlayerEquipmentManager)
    public GunData data { get; private set; } 
    
    public Transform firePos; // Điểm xuất hiện viên đạn
    public AudioSource audioSource; // Dùng để phát âm thanh

    private float nextFireTime;
    private int currentAmmoInClip;
    private int reserveAmmo;

    // Hàm khởi tạo súng bằng dữ liệu (được gọi ngay sau khi Instantiate)
    public void Initialize(GunData gunData)
    {
        data = gunData;
        currentAmmoInClip = data.maxAmmoInClip;
        reserveAmmo = data.maxReserveAmmo;
        nextFireTime = 0f;
        // Cập nhật UI ngay sau khi súng mới được trang bị
        PlayerEquipmentManager.Instance.UpdateAmmoUI(); 
    }

    public void Shoot()
    {
        if (data == null) return;

        if (Time.time < nextFireTime) return;

        if (currentAmmoInClip <= 0)
        {
            // TODO: Phát âm thanh 'click' hết đạn
            return;
        }

        // 1. Tạo viên đạn
        Instantiate(data.bulletPrefab, firePos.position, firePos.rotation);
        
        // 2. Cập nhật trạng thái
        currentAmmoInClip--;
        nextFireTime = Time.time + data.shotDelay;
        
        // 3. Cập nhật HUD sau khi bắn
        PlayerEquipmentManager.Instance.UpdateAmmoUI(); 
    }

    public void Reload()
    {
        if (data == null || reserveAmmo == 0 || currentAmmoInClip == data.maxAmmoInClip) return;

        int neededAmmo = data.maxAmmoInClip - currentAmmoInClip;
        int ammoToTransfer = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmoInClip += ammoToTransfer;
        reserveAmmo -= ammoToTransfer;
        
        // Cập nhật HUD sau khi nạp
        PlayerEquipmentManager.Instance.UpdateAmmoUI();
    }

    // Hàm lấy thông tin đạn để Manager cập nhật UI
    public (int clip, int reserve) GetAmmoInfo()
    {
        return (currentAmmoInClip, reserveAmmo);
    }
}