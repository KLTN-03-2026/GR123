using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 0f; // Đổi từ 180f thành 0f
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 26;
    public int currentAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Audio audio;


    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        RotateGun();
        Shoot();
        Reload();
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return;

        // Đổi cách tính displacement
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 displacement = mousePosition - transform.position;
        displacement.z = 0;

        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        // Kiểm tra flip
        if (angle > 90 || angle < -90)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            UpdateAmmoText();
            audio.PlayShootSound();
        }
    }

    void Reload()
    {
        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            UpdateAmmoText();
            audio.PlayReLoadSound();
        }
    }
    private void UpdateAmmoText()
{
    if (ammoText != null)
    {
        if (currentAmmo > 0)
        {
            ammoText.text = currentAmmo.ToString();  
        }
        else
        {
            ammoText.text = "0";
        }
    }
}

}
