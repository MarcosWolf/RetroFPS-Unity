using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 playerMovement = new Vector3(verticalInput, -horizontalInput, 0) * moveSpeed * Time.deltaTime;

        transform.Translate(playerMovement);
    }
}
