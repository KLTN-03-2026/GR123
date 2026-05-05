using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 0f;

    [Header("Setup")]
    public Transform firePos;
    public GameObject bulletPrefabs;
    public float shotDelay = 0.15f;

    private float nextShot;

    public int maxAmmo = 26;
    public int currentAmmo;

    [Header("External References")]
    public TextMeshProUGUI ammoText;
    public Audio audioManager;

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
        if (Camera.main == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        // Flip súng
        if (angle > 90 || angle < -90)
            transform.localScale = new Vector3(1, -1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;

            if (bulletPrefabs != null && firePos != null)
                Instantiate(bulletPrefabs, firePos.position, firePos.rotation);

            currentAmmo--;
            UpdateAmmoText();

            if (audioManager != null)
                audioManager.PlayShootSound();
        }
    }

    void Reload()
    {
        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            UpdateAmmoText();

            if (audioManager != null)
                audioManager.PlayReLoadSound();
        }
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }
}