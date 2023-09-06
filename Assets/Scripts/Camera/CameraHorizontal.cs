using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHorizontal : MonoBehaviour
{
    public float sensitivity = 2.0f; // Sensibilidade do movimento do mouse
    private float rotationX = 0;
    
    
    void Start()
    {
        
    }

    void Update()
    {

        if (GameManager.instance.isPlayerAlive)
        {
            // Player responsável pela Rotação Z (Horizontal)
            float mouseX = Input.GetAxis("Mouse X");

            mouseX *= sensitivity;

            transform.Rotate(Vector3.up * mouseX);

            rotationX -= mouseX;
            
            transform.localRotation = Quaternion.Euler(0, 0, rotationX);
        }
    }
}