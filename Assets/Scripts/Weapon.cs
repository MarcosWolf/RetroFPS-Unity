using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int weaponDamage;
    public int currentAmmo;
    public int totalAmmo;
    public int maxAmmo;

    public Camera playerCamera;
    public Animator weaponAnimator;

    public GameObject blood1;
    public GameObject blood2;

    private float AnimationRandom;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

    }
    

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
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
            }

            currentAmmo--;
            weaponAnimator.SetTrigger("WeaponFire");
        } else {
            Debug.Log("Sem munição");
        }
    }

    public void Reload()
    {
        Debug.Log("Reload");
    }
}
