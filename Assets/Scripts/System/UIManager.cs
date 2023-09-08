using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject Crosshair;

    // Camera control
    // Head bobbing
    public Animator weaponPanelAnimator;

    // Shotgun capsule
    public GameObject fallingObjectPrefab;
    public Transform capsuleSpawnPoint;

    // Bullet Casing
    public GameObject bulletCasingPrefab;
    public Transform casingSpawnPoint;

    // Blood hud when take damage
    public GameObject bloodHudPrefab;
    public Transform bloodSpawnPoint1;
    public Transform bloodSpawnPoint2;
    private float bloodAnimationRandom;

    void Awake()
    {
        instance = this;
    }

    public void SpawnBulletCasing()
    {
        GameObject newObject = Instantiate(bulletCasingPrefab, casingSpawnPoint.position, Quaternion.identity);
        newObject.transform.SetParent(transform);
    }

    public void SpawnFallingObject()
    {
        GameObject newObject = Instantiate(fallingObjectPrefab, capsuleSpawnPoint.position, Quaternion.identity);
        newObject.transform.SetParent(transform);
    }

    public void SpawnBloodHud()
    {
        bloodAnimationRandom = Random.value;

        if (bloodAnimationRandom <= 0.5f)
        {
            GameObject newObject = Instantiate(bloodHudPrefab, bloodSpawnPoint1.position, Quaternion.identity);
            newObject.transform.SetParent(transform);
        } else {
            GameObject newObject = Instantiate(bloodHudPrefab, bloodSpawnPoint2.position, Quaternion.identity);
            newObject.transform.SetParent(transform);
        }
    }

    public void headBobbing()
    {
        if (WeaponControls.instance.playerIsShooting())
        {
            if (WeaponControls.instance.currentWeaponIndex == 1)
            {
                // Shotgun
                weaponPanelAnimator.Play("ShotgunShoot");
                

            } else if (WeaponControls.instance.currentWeaponIndex == 2)
            {
                // Smg
                weaponPanelAnimator.Play("SmgShoot");
                
            }
        } else {
            if (PlayerMovement.instance.PlayerIsMoving())
            {
                weaponPanelAnimator.Play("MovingCamera");
            } else {
                weaponPanelAnimator.Play("IdleCamera");
            }
        }        

    }

    void update()
    {
    }
}
