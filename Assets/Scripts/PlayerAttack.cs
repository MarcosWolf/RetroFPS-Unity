using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Ray attackRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hitTarget;

            if (Physics.Raycast(attackRay, out hitTarget))
            {
                Debug.Log("Target hitted: " + hitTarget.transform.name);
            }
            else
            {
                Debug.Log("Nope");
            }
        }
    }
}
