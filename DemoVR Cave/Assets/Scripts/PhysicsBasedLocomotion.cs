using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsBasedLocomotion : MonoBehaviour
{
    [Header("Player References")]
    public InputActionProperty moveAction;
    public InputActionProperty sprintAction;
    public Transform headTransform;
    public Transform leftHandController;
    public Transform rightHandCController;

    [Header("Movement Parameters")]
    public float walkSpeed = 0.6f;
    public float sprintSpeed = 4.0f;
    public float gravity = 0f;

    [Header("Physics Body Setup")]
    public Transform bodyColliderTransform;
    public CapsuleCollider bodyCollider;
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2f;

    public float rbMass = 1f;
    private Rigidbody rb;

    [Header("Skill Control")]
    public bool canSprint = false;
    private Vector3 playerVelocity;
    private bool isGrounded = false;
    private float groundCheckDistance = 0.1f;

    private Transform leftController;
    private Transform rightController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            rb.mass = rbMass;
        }

        if (bodyCollider == null && bodyColliderTransform != null)
        {
            bodyCollider = bodyColliderTransform.GetComponent<CapsuleCollider>();
        }

        FindControllers();
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
        UpdateBodyCollider();
        CheckGrounded();

        
        // If the body is kinematic (in the cutscene), it doesnt process physical movement
        if (rb == null || rb.isKinematic) return;

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            bool isSprinting = canSprint && sprintAction.action.ReadValue<float>() > 0.5f;
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 forward = headTransform.forward;
            Vector3 right = headTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = forward * input.y + right * input.x;
            
            Vector3 moveVelocity = desiredMoveDirection * currentSpeed;
            moveVelocity.y = rb.linearVelocity.y; 
            
            rb.linearVelocity = moveVelocity;
        }
        else
        {
            // Stop movement but maintains gravity
            Vector3 zeroMoveVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.linearVelocity = zeroMoveVelocity;
        }
    }

    void FixedUpdate()
    {
        
        if (rb == null || rb.isKinematic) 
        {
            return;
        }

        // Applying gravity manually for physical locomotion
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }
        else
        {
            playerVelocity.y += gravity * Time.fixedDeltaTime;
        }

        Vector3 velocity = rb.linearVelocity;
        velocity.y = playerVelocity.y;
        rb.linearVelocity = velocity;

        Physics.SyncTransforms();
    }

    

    void UpdateBodyCollider()
    {
        if (bodyCollider == null || headTransform == null) return;
        float headHeightLocal = Mathf.Clamp(headTransform.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.height = headHeightLocal;
        bodyCollider.center = new Vector3(headTransform.localPosition.x, bodyCollider.height / 2f, headTransform.localPosition.z);
    }

    void CheckGrounded()
    {
        isGrounded = false;
        if (bodyCollider == null) return;
        Vector3 capsuleBottom = transform.position + bodyCollider.center - Vector3.up * (bodyCollider.height / 2f);
        if (Physics.Raycast(capsuleBottom, Vector3.down, groundCheckDistance)) isGrounded = true;
    }

    void FindControllers()
    {
        Transform xrRoot = transform.root;
        foreach (Transform child in xrRoot.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("LeftController") || child.name.Contains("Left Controller")) leftController = child;
            if (child.name.Contains("RightController") || child.name.Contains("Right Controller")) rightController = child;
        }
    }
}