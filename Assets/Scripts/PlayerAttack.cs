using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Camera playerCamera;

    public Animator weaponAnimator;

    public GameObject blood1;
    public GameObject blood2;


    // In a future update
    // Get values from the current weapon selected
    public int maxAmmo;
    public int totalAmmo;

    public int currentAmmo;


    private float AnimationRandom;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
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
                    } else {
                        Instantiate(blood2, hitTarget.point, hitTarget.transform.rotation);
                    }



                    Debug.Log("Target hitted: " + hitTarget.transform.name);

                }
                else
                {
                    Debug.Log("Nope");
                }

                
                weaponAnimator.SetTrigger("WeaponFire");
            }
            else
            {
                if (totalAmmo > 0)
                {
                    Debug.Log("Recarregue a arma! Restando " + totalAmmo + " balas");
                } else {
                    Debug.Log("Sem munição");
                }
            }
        }
    }
}
