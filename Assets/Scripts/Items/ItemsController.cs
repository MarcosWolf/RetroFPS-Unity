using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    public Weapon[] weapons;

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
                //weapons[1].GetAmmo(shotgunAmmoAmount);
                weapons[0].GetAmmo(159);
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


        Destroy(this.gameObject);
    }
}
