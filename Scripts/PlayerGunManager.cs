using UnityEngine;
using TMPro;

public class PlayerGunManager : MonoBehaviour
{
    public static PlayerGunManager Instance;

    public GameObject[] guns;
    public int currentGunIndex = -1;

    public Audio audioManager;
    public TextMeshProUGUI ammoText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EquipGun(0); // súng mặc định
    }

    public void EquipGun(int index)
    {
        if (index < 0 || index >= guns.Length) return;

        currentGunIndex = index;

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == index);
        }

        Gun gun = guns[index].GetComponent<Gun>();

        if (gun != null)
        {
            gun.audioManager = audioManager;
            gun.ammoText = ammoText;
            gun.currentAmmo = gun.maxAmmo;
        }

        Debug.Log("Equip gun index: " + index);
    }
}