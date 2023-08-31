using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;

    public Rigidbody2D playerRigidbody;

    public float playerSpeed;

    private Vector2 keyInput;
    private Vector2 mouseInput;
    public float mouseSensitivity;

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
        MovePlayer();
        RotateCamera();
    }

    private void MovePlayer()
    {
        keyInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 horizontalMovement = transform.up * -keyInput.x;
        Vector3 verticalMovement = transform.right * keyInput.y;
        
        playerRigidbody.velocity = (horizontalMovement + verticalMovement) * playerSpeed;

    }

    private void RotateCamera()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);
    }
}
