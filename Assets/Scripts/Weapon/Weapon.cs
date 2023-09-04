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

    private float lastShotTime = 0f;
    public float timeBetweenShots;

    public bool isUnlocked;

    private bool isReloading = false;

    public Camera playerCamera;
    public Animator weaponAnimator;

    public GameObject projectilePrefab;
    public float projectileSpeed;

    public GameObject blood1;
    public GameObject blood2;

    private float AnimationRandom;

    void Start()
    {
    }

    void Update()
    {
    }

    public void Shoot()
    {
        if (Time.time - lastShotTime >= timeBetweenShots)
        {
            if (isReloading) {
                isReloading = false;
            }

            if (currentAmmo > 0) {
                //Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hitTarget;
                
                Vector3 shootDirection = ray.direction.normalized;
                Vector3 playerPosition = PlayerControl.instance.transform.position;

                // Calcula a posição de início do projetil, que é a posição do jogador
                Vector3 spawnPosition = playerPosition;

                // Instancia o projetil na posição do jogador
                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);               

                // Obtém o componente Rigidbody do projetil e define a velocidade
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = shootDirection * projectileSpeed;                          

                if (Physics.Raycast(ray, out hitTarget))
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

                    if (hitTarget.transform.gameObject.CompareTag("Enemy"))
                    {
                        hitTarget.transform.gameObject.GetComponentInParent<EnemyController>().HitEnemy(weaponDamage);
                    }
                }

                lastShotTime = Time.time;
                currentAmmo--;
                SoundEffects.instance.sfxShotgunFire();
                weaponAnimator.SetTrigger("WeaponFire");
            }
            else {
                SoundEffects.instance.sfxShotgunEmpty();
            }
        }
    }

    public void Reload()
    {
        if (!isReloading && currentAmmo < ammoPerRound && totalAmmo > 0)
        {
            isReloading = true;
            weaponAnimator.SetBool("WeaponReloaded", false);

            if (weaponName == "Shotgun")
            {
                weaponAnimator.SetBool("WeaponReloaded", false);
                weaponAnimator.SetTrigger("WeaponReload");

            } else {
                weaponAnimator.SetTrigger("WeaponReload");            
            }
            
        }
    }

    public void ShotgunReload()
    {
        int ammoRemaining = ammoPerRound - currentAmmo;
        int ammoMissing = totalAmmo - ammoRemaining;

        if (ammoMissing >= 1)
        {
            currentAmmo++;
            totalAmmo--;

            Debug.Log("Total no pente:" + currentAmmo);
        } else {
            Debug.Log("Sem munição suficiente");
            weaponAnimator.SetBool("WeaponReloaded", true);
            isReloading = false;
        }

        if (currentAmmo >= ammoPerRound)
        {
            Debug.Log("Pente cheio =)");
            weaponAnimator.SetBool("WeaponReloaded", true);
            isReloading = false;
        }
        else if (totalAmmo == 0)
        {
            Debug.Log("Sem mais balas, amigo");
            weaponAnimator.SetBool("WeaponReloaded", true);
            isReloading = false;
        }
    }

    private void sfxShotgunReload()
    {
        SoundEffects.instance.sfxShotgunReload();
    }

    private void sfxShotgunPump()
    {
        SoundEffects.instance.sfxShotgunPump();
    }

    public void ReloadIsOver()
    {
        int ammoUsed = Mathf.Min(ammoPerRound - currentAmmo, totalAmmo);   
        currentAmmo += ammoUsed;
        totalAmmo -= ammoUsed;
        isReloading = false;
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
