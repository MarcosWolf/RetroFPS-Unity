using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;

    //public Animator cameraAnimator;
    public float sensitivity = 2.0f; // Sensibilidade do movimento do mouse
    private float rotationX = 0;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Player responsável pela Rotação Z (Horizontal)
        float mouseX = Input.GetAxis("Mouse X");

        mouseX *= sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseX;
        
        transform.localRotation = Quaternion.Euler(0, 0, rotationX);
    }

}
