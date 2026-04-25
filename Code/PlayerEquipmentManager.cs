using UnityEngine;
using TMPro;

public class PlayerEquipmentManager : MonoBehaviour
{
    // KHẮC PHỤC LỖI CS0103: Đảm bảo Instance là public static
    public static PlayerEquipmentManager Instance; 
    
    [Header("Unity References")]
    public Transform weaponHolder; // Điểm gắn súng
    public TextMeshProUGUI ammoDisplay; // Text hiển thị đạn

    private BaseGun currentGun;

    private void Awake()
    {
        // Logic Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (currentGun == null) return;
        
        // Bấm chuột trái để bắn
        if (Input.GetMouseButton(0))
        {
            currentGun.Shoot();
        }
        // Bấm R để nạp đạn
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentGun.Reload();
        }
    }

    // Hàm Shop gọi khi người chơi nhấn nút EQUIP
    public void EquipGun(GunData newGunData)
    {
        if (newGunData == null || newGunData.gunPrefab == null)
        {
            Debug.LogError("Không có GunData hoặc Gun Prefab để trang bị.");
            return;
        }

        // 1. Hủy súng cũ
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
            currentGun = null;
        }

        // 2. Tạo súng mới dưới Weapon Holder
        GameObject gunObject = Instantiate(newGunData.gunPrefab, weaponHolder);
        
        // 3. Lấy script và Khởi tạo
        if (gunObject.TryGetComponent<BaseGun>(out currentGun))
        {
            currentGun.Initialize(newGunData); // Truyền dữ liệu chỉ số vào súng mới
        }
        else
        {
            Debug.LogError("Prefab súng mới thiếu script BaseGun.cs!");
            Destroy(gunObject);
        }
    }
    
    // Hàm cập nhật hiển thị số đạn
    public void UpdateAmmoUI()
    {
        if (ammoDisplay == null) return;

        if (currentGun != null)
        {
            var ammoInfo = currentGun.GetAmmoInfo();
            // Định dạng: [Đạn trong băng] / [Đạn dự trữ]
            ammoDisplay.text = $"{ammoInfo.clip} / {ammoInfo.reserve}"; 
        }
        else
        {
            ammoDisplay.text = "N/A"; // Không có súng nào được trang bị
        }
    }
}