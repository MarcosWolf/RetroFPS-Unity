using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.isPlayerAlive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 inputVector = new Vector3(verticalInput, -horizontalInput, 0).normalized;
            Vector3 playerMovement = inputVector * moveSpeed * Time.deltaTime;

            //Vector3 playerMovement = new Vector3(verticalInput, -horizontalInput, 0) * moveSpeed * Time.deltaTime;

            transform.Translate(playerMovement);
        }
    }
}
