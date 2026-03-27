using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoviment : MonoBehaviour
{
    [Header("Player References")]
    public InputActionProperty moveAction;
    public InputActionProperty sprintAction;
    public Transform headTransform;

    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float sprintSpeed = 4.0f;
    public float gravityMultiplier = -9.81f; 

    [Header("Physics Body")]
    public CapsuleCollider bodyCollider;
    public float rbMass = 1f;

    [Header("Sprint")]
    public bool canSprint = false;

    private Rigidbody rb;
    private bool isGrounded = false;
    private float groundCheckDistance = 0.3f;
    private Vector3 groundNormal = Vector3.up;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.mass = rbMass;
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearDamping = 1f; 
        }
    }

    void Update()
    {
        UpdateBodyCollider();
        CheckGrounded();

        if (rb == null || rb.isKinematic || (PauseMenu.instance != null && PauseMenu.instance.isPaused)) 
        {
            if(rb != null && !rb.isKinematic) rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        Vector2 inputValues = moveAction.action.ReadValue<Vector2>();

        if (inputValues != Vector2.zero)
        {
            bool isSprinting = canSprint && sprintAction.action.ReadValue<float>() > 0.5f;
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 cameraForward = headTransform.forward;
            Vector3 cameraRight = headTransform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = (cameraForward * inputValues.y + cameraRight * inputValues.x).normalized;

            if (isGrounded)
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal).normalized;
            }

            Vector3 moveVelocity = moveDirection * currentSpeed;
            
            moveVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = moveVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void FixedUpdate()
    {
        if (rb == null || rb.isKinematic) return;

        if (isGrounded)
        {
            rb.AddForce(Vector3.down * 2f, ForceMode.Acceleration);
            
            if (rb.linearVelocity.y > 0.1f) 
            {
                Vector3 vel = rb.linearVelocity;
                vel.y *= 0.5f;
                rb.linearVelocity = vel;
            }
        }
        else
        {
            // Full gravity for falling
            rb.AddForce(Vector3.up * gravityMultiplier, ForceMode.Acceleration);
        }

        Physics.SyncTransforms();
    }

    private void UpdateBodyCollider()
    {
        if (bodyCollider == null || headTransform == null) return;
        bodyCollider.center = new Vector3(headTransform.localPosition.x, bodyCollider.height / 2f, headTransform.localPosition.z);
    }

    private void CheckGrounded()
    {
        if (bodyCollider == null) return;

        Vector3 rayStart = transform.position + Vector3.up * 0.2f;
        RaycastHit hit;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance + 0.2f))
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
    }
}