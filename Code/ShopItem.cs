using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int gunIndex;
    public int price = 100;

    public TextMeshProUGUI buttonText;

    private bool isUnlocked;

    void Start()
    {
        if (gunIndex == 0)
        {
            isUnlocked = true;
            PlayerPrefs.SetInt("GunUnlocked_0", 1);
        }
        else
        {
            isUnlocked = PlayerPrefs.GetInt("GunUnlocked_" + gunIndex, 0) == 1;
        }

        UpdateUI();
    }

    public void OnClick()
    {
        if (!isUnlocked)
            BuyGun();
        else
            EquipGun();
    }

    void BuyGun()
    {
        int coins = GameManager.Instance.GetCoins();

        if (coins < price)
        {
            Debug.Log("Không đủ tiền!");
            return;
        }

        GameManager.Instance.SpendCoins(price);

        isUnlocked = true;
        PlayerPrefs.SetInt("GunUnlocked_" + gunIndex, 1);

        EquipGun();
    }

    void EquipGun()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            player.GetComponent<PlayerGunManager>().EquipGun(gunIndex);
        }

        PlayerPrefs.SetInt("CurrentGun", gunIndex);

        UpdateUI();
    }

    void UpdateUI()
    {
        int currentGun = PlayerPrefs.GetInt("CurrentGun", 0);

        if (!isUnlocked)
            buttonText.text = "BUY " + price;
        else if (currentGun == gunIndex)
            buttonText.text = "USING";
        else
            buttonText.text = "EQUIP";
    }
}