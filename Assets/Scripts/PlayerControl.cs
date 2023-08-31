using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    public float playerSpeed;
    private Vector2 keyInputs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        keyInputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 horizontalMovement = transform.up * -keyInputs.x;
        Vector3 verticalMovement = transform.right * keyInputs.y;
        
        playerRigidbody.velocity = (horizontalMovement + verticalMovement) * playerSpeed;

    }
}
