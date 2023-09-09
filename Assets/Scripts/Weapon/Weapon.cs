using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int weaponDamage;
    public int currentAmmo;
    public int totalAmmo;
    public int ammoPerRound;
    public int maxAmmo;

    public bool isContinuous;

    private bool isShootingContinuous = false;
    private float timeBetweenContinuousShots = 0.13f; // Intervalo de tempo entre tiros contínuos
    private float lastContinuousShotTime = 0f;

    private float lastShotTime = 0f;
    public float timeBetweenShots;

    public bool isUnlocked;

    public bool isReloading = false;
    private bool pumpReload;

    private bool canShot;

    public Camera playerCamera;
    public Animator weaponAnimator;

    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float spawnOffset;
    public float spreadAngle;

    public GameObject blood1;
    public GameObject blood2;

    private float AnimationRandom;

    void Start()
    {
        isReloading = false;
        canShot = true;
    }

    void Update()
    {
        setCrosshair();
    }

    public void setCrosshair()
    {
        if (weaponName == "Smg")
        {
            if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("SubmachineReload"))
            {
                UIManager.instance.Crosshair.gameObject.SetActive(false);
            }
            else if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("SubmachineSlide"))
            {
                UIManager.instance.Crosshair.gameObject.SetActive(false);
            }
            else
            {
                UIManager.instance.Crosshair.gameObject.SetActive(true);
            }
        }
        else if (weaponName == "Shotgun")
        {
            if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShotgunReload"))
            {
                UIManager.instance.Crosshair.gameObject.SetActive(false);
            }
            else if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShotgunForend"))
            {
                UIManager.instance.Crosshair.gameObject.SetActive(false);
            }
            else {
                UIManager.instance.Crosshair.gameObject.SetActive(true);
            }
        }
    }

    public void Shoot()
    {
        if (GameManager.instance.isPlayerAlive) {
            if (!WeaponControls.instance.playerIsRemovingWeapon() && !WeaponControls.instance.playerIsGettingWeapon() && WeaponControls.instance.isWeaponActive())
            {
                if (Time.time - lastShotTime >= timeBetweenShots)
                {
                    if (isReloading) {
                        isReloading = false;
                    }

                    if (weaponName == "Smg") {
                        if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("SubmachineSlide")) {
                            canShot = false;
                        } else {
                            canShot = true;
                        }
                    }

                    if (weaponName == "Shotgun") {
                        if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShotgunForend")) {
                            canShot = false;
                        } else {
                            canShot = true;
                        }
                        
                    }

                    if (currentAmmo > 0 && canShot) {
                        //Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                        RaycastHit hitTarget;
                        
                        Vector3 shootDirection = ray.direction.normalized;
                        Vector3 playerPosition = PlayerControl.instance.transform.position;

                        int obstacleLayerMask = LayerMask.GetMask("Obstacle");



                        if (weaponName == "Smg")
                        {
                            float randomSpreadAngle = Random.Range(-spreadAngle, spreadAngle);

                            Vector3 randomSpreadDirection = Quaternion.Euler(0f, 0f, randomSpreadAngle) * shootDirection;

                            // Calcula a posição de início do projetil, que é a posição do jogador
                            Vector3 spawnPosition = playerPosition + (randomSpreadDirection * spawnOffset);

                            // Instancia o projetil na posição do jogador
                            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);               

                            // Obtém o componente Rigidbody do projetil e define a velocidade
                            Rigidbody rb = projectile.GetComponent<Rigidbody>();
                            rb.velocity = randomSpreadDirection * projectileSpeed;     
                        } else if (weaponName == "Shotgun")
                        {
                            float spacing = 1f;

                            for (int i = -1; i <= 1; i++)
                            {
                                float randomSpreadAngle = Random.Range(-spreadAngle, spreadAngle);
                                Vector3 randomSpreadDirection = Quaternion.Euler(0f, 0f, randomSpreadAngle) * shootDirection;

                                Vector3 initialSpawnPosition = playerPosition + (randomSpreadDirection * spawnOffset);

                                Vector3 spawnPosition = initialSpawnPosition + (randomSpreadDirection * spacing * i);

                                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

                                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                                rb.velocity = randomSpreadDirection * projectileSpeed;
                            }
                        }

                        
                        if (Physics.Raycast(ray, out hitTarget))
                        {                    
                            
                            if (hitTarget.collider.CompareTag("Enemy"))
                            {

                                AnimationRandom = Random.value;

                                if (AnimationRandom <= 0.5f)
                                {
                                    Instantiate(blood1, hitTarget.point, hitTarget.transform.rotation);
                                }
                                else
                                {
                                    Instantiate(blood2, hitTarget.point, hitTarget.transform.rotation);
                                }

                                hitTarget.transform.gameObject.GetComponentInParent<EnemyController>().HitEnemy(weaponDamage);
                            }
                        }

                        pumpReload = false;
                        lastShotTime = Time.time;
                        currentAmmo--;
                        if (weaponName == "Shotgun") {
                            SoundEffects.instance.sfxShotgunFire();
                        } else if (weaponName == "Smg") {
                            SoundEffects.instance.sfxSmgFire();
                            UIManager.instance.SpawnBulletCasing();
                            SoundEffects.instance.sfxCasingDrop();
                        }
                        weaponAnimator.SetTrigger("WeaponFire");
                    }
                    else {
                        SoundEffects.instance.sfxWeaponEmpty();
                    }
                }
            }
        }
    }

    public void StartShootingContinuous()
    {
        if (!isReloading && Time.time - lastContinuousShotTime >= timeBetweenContinuousShots)
        {
            isShootingContinuous = true;
            StartCoroutine(ContinuousShooting());
        }
    }

    public void StopShootingContinuous()
    {
        isShootingContinuous = false;
    }

    private IEnumerator ContinuousShooting()
    {
        while (isShootingContinuous)
        {
            Shoot();

            lastContinuousShotTime = Time.time;

            yield return new WaitForSeconds(timeBetweenContinuousShots);
        }
    }


    public void Reload()
    {
        if (GameManager.instance.isPlayerAlive) {
            if (!isReloading && currentAmmo < ammoPerRound && totalAmmo > 0)
            {
                
                isReloading = true;
                weaponAnimator.SetBool("WeaponReloaded", false);

                if (weaponName == "Shotgun")
                {
                    weaponAnimator.SetTrigger("WeaponReload");
                    pumpReload = true;

                } else if (weaponName == "Smg") {
                    weaponAnimator.SetTrigger("WeaponReload");            
                }
                
            }
        }
    }

    public void ShotgunReload()
    {
        
        int ammoRemaining = ammoPerRound - currentAmmo;
        int ammoMissing = totalAmmo - ammoRemaining;

        //if (ammoMissing >= 1)
        if (currentAmmo < ammoPerRound)
        {
            currentAmmo++;
            totalAmmo--;
        } else {
            weaponAnimator.SetBool("WeaponReloaded", true);
            isReloading = false;
        }

        if (currentAmmo >= ammoPerRound)
        {
            weaponAnimator.SetBool("WeaponReloaded", true);
            isReloading = false;
        }
        else if (totalAmmo == 0)
        {
            weaponAnimator.SetBool("WeaponReloaded", true);
            isReloading = false;
        }
    }

    public void SmgReload()
    {
        int ammoRemaining = ammoPerRound - currentAmmo;
        int ammoMissing = totalAmmo - ammoRemaining;

        ReloadIsOver();

        weaponAnimator.SetBool("WeaponReloaded", true);
        isReloading = false;
    }

    private void sfxShotgunReload()
    {
        SoundEffects.instance.sfxShotgunReload();
    }

    private void sfxShotgunPump()
    {
        SoundEffects.instance.sfxShotgunPump();

        if (!pumpReload) {
            UIManager.instance.SpawnFallingObject();
            SoundEffects.instance.sfxShellDrop();
        }
    }

    private void sfxSmgReload1()
    {
        SoundEffects.instance.sfxSmgReload1();
    }

    private void sfxSmgReload2()
    {
        SoundEffects.instance.sfxSmgReload2();
    }

    private void sfxSmgSlide()
    {
        SoundEffects.instance.sfxSmgSlide();
    }

    public void ReloadIsOver()
    {
        int ammoUsed = Mathf.Min(ammoPerRound - currentAmmo, totalAmmo);   
        currentAmmo += ammoUsed;
        totalAmmo -= ammoUsed;
    }

    public bool GetAmmo(int ammoAmount)
    {
        if (totalAmmo < maxAmmo)
        {
            int ammoToAdd = Mathf.Min(ammoAmount, maxAmmo - totalAmmo);
            totalAmmo += ammoToAdd;
            return true;
        }
        else
        {
            return false;
        }
    }
}
