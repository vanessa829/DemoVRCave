using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PhysicsBasedLocomotion : MonoBehaviour
{
    [Header("Referências de Input e Jogador")]
    [Tooltip("A ação de input para o movimento do analógico (ex: XRI LeftHand/Move).")]
    public InputActionProperty moveAction;

    [Header("Input Actions")]
    [Tooltip("A ação de input para correr (ex: PlayerGlobal/Sprint).")]
    [SerializeField] private InputActionReference sprintAction; 

    [Tooltip("O Transform da câmara do jogador para determinar a direção 'frente'.")]
    public Transform headTransform;

    [Header("Parâmetros de Movimento")]
    [Tooltip("A velocidade de caminhada do jogador em metros por segundo.")]
    public float walkSpeed = 2.0f;

    [Tooltip("A velocidade de corrida do jogador em metros por segundo.")]
    public float sprintSpeed = 4.0f; 

    [Tooltip("A força da gravidade a ser aplicada ao jogador.")]
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 playerVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable(); // <-- ATIVAR A NOVA AÇÃO
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        sprintAction.action.Disable(); // <-- DESATIVAR A NOVA AÇÃO
    }

    void Update()
    {
        // --- PARTE 1: GRAVIDADE (sem alterações) ---
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // --- PARTE 2: MOVIMENTO DO JOGADOR (com alterações) ---
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            return;
        }

        // --- NOVA LÓGICA DE CORRIDA ---
        // Verifica se a ação de correr está a ser pressionada
        bool isSprinting = sprintAction.action.ReadValue<float>() > 0.5f;
        
        // Escolhe a velocidade correta com base no estado de corrida
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        // -----------------------------

        Vector3 forward = headTransform.forward;
        Vector3 right = headTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * input.y + right * input.x;

        // Usa a 'currentSpeed' em vez da 'moveSpeed' fixa
        characterController.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
    }
}