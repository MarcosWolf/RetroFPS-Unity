using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeJump : MonoBehaviour
{
    public float jumpHeight = 1.0f; // Altura do pulo falso
    public float jumpDuration = 0.5f; // Duração do pulo falso

    private bool isJumping = false;
    private Vector3 originalPosition;
    private float jumpTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            // Iniciar o pulo falso
            isJumping = true;
            jumpTimer = 0.0f;
        }

        if (isJumping)
        {
            // Incrementar o temporizador do pulo
            jumpTimer += Time.deltaTime;

            // Calcular a nova posição do jogador para o pulo falso
            Vector3 jumpPosition = originalPosition + Vector3.back * jumpHeight * Mathf.Sin((jumpTimer / jumpDuration) * Mathf.PI);

            // Atualizar a posição do jogador
            transform.position = jumpPosition;

            // Verificar se o pulo falso terminou
            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                transform.position = originalPosition; // Restaurar a posição original
            }
        }
    }
}
