using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVertical : MonoBehaviour
{
    public float sensitivity = 2.0f; // Sensibilidade do movimento do mouse
    public float minYAngle = 50.0f; // Ângulo mínimo de rotação vertical
    public float maxYAngle = 160.0f; // Ângulo máximo de rotação vertical
    private float rotationY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera responsável pela Rotação Y (Vertical)
        float mouseY = Input.GetAxis("Mouse Y");

        mouseY *= sensitivity;

        rotationY+= mouseY;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        transform.localRotation = Quaternion.Euler(0, rotationY, -90);
    }
}