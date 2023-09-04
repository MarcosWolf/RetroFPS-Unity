using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeJump : MonoBehaviour
{
    public float jumpHeight = 1.0f; // Altura do pulo falso
    public float jumpDuration = 0.5f; // Duração do pulo falso

    private bool isJumping = false;
    private float originalZPosition;
    private float jumpTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        originalZPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlayerAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                //originalPosition = transform.position.z;
                // Iniciar o pulo falso
                isJumping = true;
                jumpTimer = 0.0f;
            }

            if (isJumping)
            {
                // Incrementar o temporizador do pulo
                jumpTimer += Time.deltaTime;

                // Calcular a nova posição Y do jogador para o pulo falso
                float jumpZPosition = originalZPosition - jumpHeight * Mathf.Sin((jumpTimer / jumpDuration) * Mathf.PI);

                // Atualizar a posição do jogador apenas no eixo Y
                Vector3 newPosition = transform.position;
                newPosition.z = jumpZPosition;
                transform.position = newPosition;

                // Verificar se o pulo falso terminou
                if (jumpTimer >= jumpDuration)
                {
                    isJumping = false;
                    newPosition.z = originalZPosition; // Definir a posição Y de volta ao chão
                    transform.position = newPosition;
                }
            }
        }
    }
}
