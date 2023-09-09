using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponControls : MonoBehaviour
{
    public static WeaponControls instance;

    public Weapon[] weapons;
    public int currentWeaponIndex;
    public bool isShooting;

    public bool isRemovingWeapon;
    public bool isGettingWeapon;

    // Hud
    public TMP_Text ammoText;
    public TMP_Text weaponText;

    public delegate void AnimationFinishedHandler();

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
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
            isShooting = true;

            if (weapons[currentWeaponIndex].isContinuous)
            {
                weapons[currentWeaponIndex].StartShootingContinuous();
            } else {
                weapons[currentWeaponIndex].Shoot();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;

            if (weapons[currentWeaponIndex].isContinuous)
            {
                weapons[currentWeaponIndex].StopShootingContinuous();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapons[currentWeaponIndex].Reload();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            weapons[currentWeaponIndex].GetAmmo(12);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            UIManager.instance.SpawnBulletCasing();
        }
    }


    private void UpdateHud()
    {
        weaponText.text = weapons[currentWeaponIndex].weaponName;
        ammoText.text = weapons[currentWeaponIndex].currentAmmo + "/" + weapons[currentWeaponIndex].totalAmmo;
    }

    private void SwitchWeapon(int newWeaponIndex)
    {
        if (GameManager.instance.isPlayerAlive)
        {
            if (weapons[currentWeaponIndex].isReloading == false )
            {
                if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length)
                {
                    if (currentWeaponIndex != newWeaponIndex)
                    {
                        if (weapons[newWeaponIndex].isUnlocked) {
                            isRemovingWeapon = true;

                            StartCoroutine(WaitForRemovingWeaponToFinish(() =>
                            {
                                //SetCanvasImageVisibility(weapons[currentWeaponIndex].gameObject, false);
                                SetObjectVisibility(weapons[currentWeaponIndex].gameObject, false);
                                Debug.Log("Antiga arma era: " + currentWeaponIndex);
                                //weapons[currentWeaponIndex].gameObject.SetActive(false);
                                currentWeaponIndex = newWeaponIndex;
                                Debug.Log("Arma atual é: " + currentWeaponIndex);
                                isRemovingWeapon = false;

                                StartCoroutine(ActivateNewWeapon());
                            }));
                        } else {
                            Debug.Log("Arma bloqueada ");
                        }
                    }
                }
            }
        }
    }

    private IEnumerator WaitForRemovingWeaponToFinish(System.Action onAnimationFinished)
    {
        // Espere até que a animação "RemovingWeapon" tenha terminado
        while (!UIManager.instance.HasAnimationFinished("RemovingWeapon"))
        {
            yield return null;
        }

        // Chame o evento quando a animação terminar
        onAnimationFinished?.Invoke();
    }

    private IEnumerator WaitForGettingWeaponToFinish(System.Action onAnimationFinished)
    {
        // Espere até que a animação "RemovingWeapon" tenha terminado
        while (!UIManager.instance.HasAnimationFinished("GettingWeapon"))
        {
            yield return null;
        }

        // Chame o evento quando a animação terminar
        onAnimationFinished?.Invoke();
    }

    private IEnumerator ActivateNewWeapon()
    {
        yield return new WaitForSeconds(0.1f);

        Image image = weapons[currentWeaponIndex].GetComponent<Image>();

        isGettingWeapon = true;
        
        yield return new WaitForSeconds(0.1f);

        if (image != null)
        {
            image.enabled = true;
        }

        StartCoroutine(WaitForGettingWeaponToFinish(() =>
        {
            isGettingWeapon = false;
            UIManager.instance.Crosshair.gameObject.SetActive(true);
            SoundEffects.instance.sfxSwapWeapon();
        }));
    }

    private void SetObjectVisibility(GameObject obj, bool isVisible)
    {
        // Obtém o componente Renderer do objeto
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            // Define a visibilidade do componente Renderer
            renderer.enabled = isVisible;
        }

        // Obtém o componente Image do objeto
        Image image = obj.GetComponent<Image>();

        if (image != null)
        {
            // Define a visibilidade da imagem
            image.enabled = isVisible;
        }
    }

    public bool isWeaponActive()
    {
        GameObject obj = weapons[currentWeaponIndex].gameObject;
        Image image = obj.GetComponent<Image>();

        if (image.enabled)
        {
            return true;
        } else {
            return false;
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

    public bool playerIsRemovingWeapon()
    {
        if (isRemovingWeapon == true)
        {
            return true;
        } else {
            return false;
        }
    }

    public bool playerIsGettingWeapon()
    {
        if (isGettingWeapon == true)
        {
            return true;
        } else {
            return false;
        }
    }

    public bool playerIsShooting()
    {
        if (isShooting == true)
        {
            if (weapons[currentWeaponIndex].isContinuous)
            {
                return true;
            }
            else {
                return false;
            }
        } else {
            return false;
        }
    }
}
