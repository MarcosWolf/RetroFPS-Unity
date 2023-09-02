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

    public bool isUnlocked;

    private bool isReloading = false;

    public Camera playerCamera;
    public Animator weaponAnimator;

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
        if (!isReloading)
        {
            if (currentAmmo > 0) {
                Ray attackRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hitTarget;

                if (Physics.Raycast(attackRay, out hitTarget))
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

                currentAmmo--;
                weaponAnimator.SetTrigger("WeaponFire");
            }
            else {
            Debug.Log("Sem munição");
            //Tocar som de clique
            }
        }
    }

    public void Reload()
    {
        if (!isReloading && currentAmmo < ammoPerRound && totalAmmo > 0)
        {
            isReloading = true;
            weaponAnimator.SetTrigger("WeaponReload");            
            Debug.Log("Carregando");
            
        }
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
