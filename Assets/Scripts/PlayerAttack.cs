using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Camera playerCamera;

    // In a future update
    // Get values from the current weapon selected
    public int maxAmmo;
    public int currentAmmo = 10;

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
                currentAmmo -= 1;

                if (Physics.Raycast(attackRay, out hitTarget))
                {
                    Debug.Log("Target hitted: " + hitTarget.transform.name);
                }
                else
                {
                    Debug.Log("Nope");
                }
            }
            else
            {
                Debug.Log("Sem munição");
            }
        }
    }
}
