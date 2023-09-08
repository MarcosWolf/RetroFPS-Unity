using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector3 moveDirection;

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

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        moveDirection = cameraForward * verticalInput + cameraRight * horizontalInput;

        if (moveDirection.sqrMagnitude < 0.01f)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isPlayerAlive)
        {
            Vector2 clampedVelocity = Vector2.ClampMagnitude(moveDirection * moveSpeed, moveSpeed);
            rb.velocity = clampedVelocity;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
    }
}
