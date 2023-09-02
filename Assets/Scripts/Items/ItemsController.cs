using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private WeaponControls weaponControls;

    // AMMO
    public bool shotgunAmmoItem;

    // HEALTH
    public bool medicalKitItem;

    // SHIELD
    
    // KEYS
    public bool blueKeyItem;
    public bool yellowKeyItem;
    public bool redKeyItem;

    // Item Amount
    public int shotgunAmmoAmount;

    public int medicalKitAmount;

    // Start is called before the first frame update
    void Start()
    {
        weaponControls = FindObjectOfType<WeaponControls>();
    }

    // Update is called once per frame
    void Update()
    {        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Ammo
            if (shotgunAmmoItem == true)
            {
                if (weaponControls.getAmmo(1, 12)) {
                    Destroy(this.gameObject);
                } else {
                    Debug.Log("Munição cheia");
                }
            }

            // Health
            if (medicalKitItem == true)
            {
                Debug.Log("Curando");
            }

            // Keys
            if (blueKeyItem == true)
            {

            }

            if (yellowKeyItem == true)
            {

            }

            if (redKeyItem == true)
            {

            }
        }

    }
}
