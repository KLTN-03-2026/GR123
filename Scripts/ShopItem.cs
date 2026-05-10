using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("Gun Info")]
    public int gunIndex;
    public int price = 100;

    [Header("UI")]
    public TextMeshProUGUI buttonText;

    private bool isUnlocked;

    void Start()
    {
        // Súng đầu mặc định mở
        if (gunIndex == 0)
        {
            isUnlocked = true;

            PlayerPrefs.SetInt("GunUnlocked_0", 1);

            // Nếu chưa có súng đang dùng
            if (!PlayerPrefs.HasKey("CurrentGun"))
            {
                PlayerPrefs.SetInt("CurrentGun", 0);
            }
        }
        else
        {
            isUnlocked =
                PlayerPrefs.GetInt("GunUnlocked_" + gunIndex, 0) == 1;
        }

        UpdateUI();
    }

    public void OnClick()
    {
        // Chưa mua -> mua
        if (!isUnlocked)
        {
            BuyGun();
        }
        // Đã mua -> equip
        else
        {
            EquipGun();
        }
    }

    void BuyGun()
    {
        int coins = GameManager.Instance.GetCoins();

        // Không đủ tiền
        if (coins < price)
        {
            Debug.Log("Không đủ tiền!");
            return;
        }

        // Trừ tiền
        GameManager.Instance.SpendCoins(price);

        // Unlock súng
        isUnlocked = true;

        PlayerPrefs.SetInt("GunUnlocked_" + gunIndex, 1);

        // Sau khi mua -> tự equip
        EquipGun();
    }

    void EquipGun()
    {
        // Lưu súng đang dùng
        PlayerPrefs.SetInt("CurrentGun", gunIndex);

        // Equip cho player
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            PlayerGunManager gunManager =
                player.GetComponent<PlayerGunManager>();

            if (gunManager != null)
            {
                gunManager.EquipGun(gunIndex);
            }
        }

        // Update tất cả item shop
        ShopItem[] allItems = FindObjectsOfType<ShopItem>();

        foreach (ShopItem item in allItems)
        {
            item.RefreshData();
            item.UpdateUI();
        }
    }

    public void RefreshData()
    {
        isUnlocked =
            PlayerPrefs.GetInt("GunUnlocked_" + gunIndex, gunIndex == 0 ? 1 : 0) == 1;
    }

    public void UpdateUI()
    {
        int currentGun = PlayerPrefs.GetInt("CurrentGun", 0);

        // Chưa mua
        if (!isUnlocked)
        {
            buttonText.text = "BUY " + price;
        }
        // Đang dùng
        else if (currentGun == gunIndex)
        {
            buttonText.text = "USING";
        }
        // Đã mua nhưng chưa dùng
        else
        {
            buttonText.text = "EQUIP";
        }
    }
}