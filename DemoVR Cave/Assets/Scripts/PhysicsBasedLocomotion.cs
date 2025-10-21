using UnityEngine;
using UnityEngine.InputSystem;

// Garante que este script está sempre num objeto com um Character Controller.
// Isto adiciona o componente automaticamente se ele não existir.
[RequireComponent(typeof(CharacterController))]
public class PhysicsBasedLocomotion : MonoBehaviour
{
    [Header("Referências de Input e Jogador")]
    [Tooltip("A ação de input para o movimento do analógico (ex: XRI LeftHand/Move).")]
    public InputActionProperty moveAction;

    [Tooltip("O Transform da câmara do jogador para determinar a direção 'frente'.")]
    public Transform headTransform;

    [Header("Parâmetros de Movimento")]
    [Tooltip("A velocidade de caminhada do jogador em metros por segundo.")]
    public float moveSpeed = 2.0f;

    [Tooltip("A força da gravidade a ser aplicada ao jogador.")]
    public float gravity = -9.81f;

    // Referência privada para o Character Controller, obtida automaticamente.
    private CharacterController characterController;

    // Vetor para acumular a velocidade vertical (gravidade).
    private Vector3 playerVelocity;

    void Awake()
    {
        // Obtém a referência do Character Controller que está no mesmo objeto.
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Ativa a ação de input quando o script é ativado.
        moveAction.action.Enable();
    }



    private void OnDisable()
    {
        // Desativa a ação de input quando o script é desativado.
        moveAction.action.Disable();
    }

    void Update()
    {
        Debug.Log("O SCRIPT PhysicsBasedLocomotion ESTÁ A FUNCIONAR!");
        // --- PARTE 1: GRAVIDADE ---
        // Verifica se o Character Controller está no chão.
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            // Se estiver no chão, reseta a velocidade vertical para um valor baixo.
            // Isto evita que a gravidade se acumule infinitamente.
            playerVelocity.y = -2f;
        }

        // Aplica a aceleração da gravidade à velocidade vertical a cada segundo.
        playerVelocity.y += gravity * Time.deltaTime;

        // Move o jogador para baixo de acordo com a gravidade.
        // Esta chamada de Move() também deteta colisões com o chão.
        characterController.Move(playerVelocity * Time.deltaTime);


        // --- PARTE 2: MOVIMENTO DO JOGADOR ---
        // Lê o valor do analógico (um Vector2, onde Y é frente/trás e X é esquerda/direita).
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Se não houver input do analógico, não há mais nada a fazer nesta frame.
        if (input == Vector2.zero)
        {
            return;
        }

        // Obtém as direções "frente" e "direita" com base na rotação da cabeça do jogador.
        Vector3 forward = headTransform.forward;
        Vector3 right = headTransform.right;

        // Anula a componente vertical para que o jogador não voe ou se enterre ao olhar para cima/baixo.
        forward.y = 0;
        right.y = 0;

        // Normaliza os vetores para garantir uma velocidade consistente, mesmo em movimento diagonal.
        forward.Normalize();
        right.Normalize();

        // Calcula a direção final do movimento combinando as direções com o input do analógico.
        Vector3 desiredMoveDirection = forward * input.y + right * input.x;

        // A MÁGICA ACONTECE AQUI:
        // A função characterController.Move() move o jogador, mas para automaticamente
        // se encontrar um colisor (porta, parede, etc.) no seu caminho.
        characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);
    }
}