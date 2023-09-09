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

    // Weapon control

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

    public void removingWeapon()
    {
        weaponPanelAnimator.Play("RemovingWeapon");
    }

    public void gettingWeapon()
    {
        weaponPanelAnimator.Play("GettingWeapon");
    }

    public bool HasAnimationFinished(string animationName)
    {
        // Obtém informações do estado atual da animação
        AnimatorStateInfo stateInfo = weaponPanelAnimator.GetCurrentAnimatorStateInfo(0);

        // Verifica se o nome do estado atual corresponde ao nome da animação
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    public void headBobbing()
    {
        if (WeaponControls.instance.playerIsRemovingWeapon())
        {
            removingWeapon();
        }
        else if (WeaponControls.instance.playerIsGettingWeapon())
        {
            gettingWeapon();
        }
        else {
            if (WeaponControls.instance.playerIsShooting())
            {
                if (WeaponControls.instance.setRecoil == true)
                {
                    Debug.Log("Aqui");
                    if (WeaponControls.instance.currentWeaponIndex == 1)
                    {
                        weaponPanelAnimator.Play("ShotgunShoot");
                    } else if (WeaponControls.instance.currentWeaponIndex == 2)
                    {
                        weaponPanelAnimator.Play("SmgShoot");
                    }
                }
            }
            else {

                if (PlayerMovement.instance.PlayerIsMoving())
                {
                    weaponPanelAnimator.Play("MovingCamera");
                } else {
                    weaponPanelAnimator.Play("IdleCamera");
                }
            }
        }
    }

    void update()
    {
    }
}
