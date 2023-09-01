using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControls : MonoBehaviour
{
    public Weapon[] weapons;
    private int currentWeaponIndex;

    
    void Start()
    {
        currentWeaponIndex = 1;    //Shotgun
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetButtonDown("Fire1"))
        {
            weapons[currentWeaponIndex].Shoot();
        }
    }

    private void SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length)
        {
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            currentWeaponIndex = newWeaponIndex;
            weapons[currentWeaponIndex].gameObject.SetActive(true);
        }
    }
}
