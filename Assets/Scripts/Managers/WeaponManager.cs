using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] AudioClip emptyAmmo;

    public AudioClip magIn;
    public AudioClip magOut;
    public AudioClip releaseSlide;
    public AudioClip gunMode;
    AudioSource audioSource;
    AmmoManager ammo;

    WeaponRecoil recoil;
    public bool Msg;

    [Header("Muzzle Flash ")]
    ParticleSystem muzzleFlashParticle;
    Light muzzleFlashLight;
    [SerializeField] float lightIntensity = 2f;
    float lightReturnSpeed = 20f;

    WeaponBloom bloom;

    void Start()
    {
        recoil = GetComponent<WeaponRecoil>();
        ammo = GetComponent<AmmoManager>();
        audioSource = GetComponent<AudioSource>();
        muzzleFlashParticle = GetComponentInChildren<ParticleSystem>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        bloom = GetComponent<WeaponBloom>();
        aim = GetComponentInParent<AimStateManager>();
        fireRateTimer = fireRate; //cause first bullet to be shooted without any delay

        if (!muzzleFlashLight)
        {
            Debug.Log("Light Missing");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutomatic && Msg)
            Debug.Log("FireMode : Auto");
        if (!isAutomatic && Msg)
            Debug.Log("FireMode : Single");

        if (Input.GetKeyDown(KeyCode.B))
        {
            isAutomatic = !isAutomatic;
            audioSource.PlayOneShot(gunMode);
        }
        if (ShouldFire()) Fire();
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }
    // bool ShouldFire()
    // {
    //     fireRateTimer += Time.deltaTime;
    //     if (fireRateTimer < fireRate) return false;
    //     if (ammo.currentAmmo == 0)
    //       return false;
    //     if (!isAutomatic && Input.GetKeyDown(KeyCode.Mouse0)) return true;
    //     if (isAutomatic && Input.GetKey(KeyCode.Mouse0)) return true;
    //     return false;
    // }
    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;

        bool wantsToFire = (!isAutomatic && Input.GetKeyDown(KeyCode.Mouse0)) || (isAutomatic && Input.GetKey(KeyCode.Mouse0));

        if (!wantsToFire) return false;

        if (ammo.currentAmmo == 0)
        {
            audioSource.PlayOneShot(emptyAmmo);
            fireRateTimer = 0f;
            return false;
        }
        return true;
    }

    void Fire()
    {
        fireRateTimer = 0f;
        barrelPos.LookAt(aim.aimPos);//focus on aim position 
        //adding ammo spread
        barrelPos.localEulerAngles = bloom.BloomAngles(barrelPos);
        // Debug.Log("Fire");
        ammo.currentAmmo -= bullerPerShot; ;
        //play gunshot sound
        audioSource.PlayOneShot(gunShoot);
        recoil.TriggerRecoil();
        TriggerMuzzleFlash();
        for (int i = 0; i < bullerPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVeclocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticle.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
