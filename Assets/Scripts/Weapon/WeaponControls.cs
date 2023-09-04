using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponControls : MonoBehaviour
{
    public Weapon[] weapons;
    public int currentWeaponIndex;

    // Hud
    public TMP_Text ammoText;
    public TMP_Text weaponText;
    
    void Start()
    {
        SwitchWeapon(1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHud();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))    
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            weapons[currentWeaponIndex].Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapons[currentWeaponIndex].Reload();
            //StartCoroutine(weapons[currentWeaponIndex].Reload());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            weapons[currentWeaponIndex].GetAmmo(12);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            
        }
    }


    private void UpdateHud()
    {
        weaponText.text = weapons[currentWeaponIndex].weaponName;
        ammoText.text = weapons[currentWeaponIndex].currentAmmo + "/" + weapons[currentWeaponIndex].totalAmmo;
    }

    private void SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length)
        {
            if (currentWeaponIndex != newWeaponIndex)
            {
                if (weapons[newWeaponIndex].isUnlocked) {
                    weapons[currentWeaponIndex].gameObject.SetActive(false);
                    currentWeaponIndex = newWeaponIndex;
                    weapons[currentWeaponIndex].gameObject.SetActive(true);
                } else {
                    Debug.Log("Arma bloqueada ");
                }
            }
        }
    }

    public void InactiveWeapon()
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
    }

    public bool getAmmo(int weapon, int ammoAmount)
    {
        if (weapons[weapon].GetAmmo(ammoAmount))
        {
            return true;
        } else {
            return false;
        }
    }
}
