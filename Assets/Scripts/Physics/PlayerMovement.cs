using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.z = 0;
        cameraRight.z = 0;

        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        Vector3 movement = new Vector3(verticalInput, -horizontalInput, 0f) * moveSpeed;

        // Aplica a força ao Rigidbody para mover o personagem
        if (GameManager.instance.isPlayerAlive)
        {
            rb.velocity = moveDirection * moveSpeed;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

}