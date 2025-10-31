using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PhysicsBasedLocomotion : MonoBehaviour
{
    [Header("Referências de Input e Jogador")]
    public InputActionProperty moveAction;
    public InputActionProperty sprintAction;
    public Transform headTransform;

    [Header("Parâmetros de Movimento")]
    public float walkSpeed = 0.6f;
    public float sprintSpeed = 4.0f;
    public float gravity = 0;


    [Header("Controlo de Habilidade")]
    [Tooltip("Controla se o jogador tem a habilidade de correr.")]
    public bool canSprint = false; 

    private CharacterController characterController;
    private Vector3 playerVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        sprintAction.action.Disable();
    }

    void Update()
    {
        // Gravidade (se gravity for diferente de 0)
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Movimento
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            return;
        }

        
        // Verifica se a ação de correr está a ser pressionada E se o jogador PODE correr
        bool isSprinting = canSprint && sprintAction.action.ReadValue<float>() > 0.5f;
        
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        // ------------------------------------

        Vector3 forward = headTransform.forward;
        Vector3 right = headTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * input.y + right * input.x;
        characterController.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
    }
}