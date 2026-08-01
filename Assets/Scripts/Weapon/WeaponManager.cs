using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate Properties")]
    [SerializeField] float fireRate;
    float fireRateTimer;
    [Header("Weapon Fire Type")]
    [SerializeField] bool isAutomatic;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVeclocity;
    [SerializeField] int bullerPerShot = 1;
    AimStateManager aim;

    [Header("Audio Section")]
    [SerializeField] AudioClip gunShoot;
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        fireRateTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) isAutomatic = !isAutomatic;
        if (ShouldFire()) Fire();
    }
    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (!isAutomatic && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (isAutomatic && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }
    void Fire()
    {
        fireRateTimer = 0f;
        barrelPos.LookAt(aim.aimPos);
        // Debug.Log("Fire");
        //play gunshot sound
        audioSource.PlayOneShot(gunShoot);
        for (int i = 0; i < bullerPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVeclocity, ForceMode.Impulse);
        }
    }
}
