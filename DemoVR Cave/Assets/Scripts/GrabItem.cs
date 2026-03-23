using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class GrabItem : MonoBehaviour
{
    [Header("Return Settings")]
    public float delayBeforeReturn = 3.0f;

    [Header("Physics Settings")]
    public bool isKinematicOnGrab = true;
    public bool isKinematicOnReturn = true;
    public bool useGravityOnRelease = true;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Rigidbody physicsBody;
    private XRGrabInteractable grabComponent;
    private Coroutine returnProcess;
    private Collider bodyCollider;
    
    private bool readyToRelease = false;
    private bool isStickyActive = false;

    void Awake()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        
        physicsBody = GetComponent<Rigidbody>();
        grabComponent = GetComponent<XRGrabInteractable>();

        if (physicsBody != null)
        {
            physicsBody.isKinematic = isKinematicOnReturn;
        }

        var locomotion = Object.FindFirstObjectByType<PhysicsBasedLocomotion>();
        if (locomotion != null) 
        {
            bodyCollider = locomotion.bodyCollider;
        }
    }

    void OnEnable()
    {
        grabComponent.selectEntered.AddListener(OnGrab);
        grabComponent.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabComponent.selectEntered.RemoveListener(OnGrab);
        grabComponent.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // If we are already in sticky mode, this second click means we want to release
        if (isStickyActive)
        {
            readyToRelease = true;
            // Force the interactor to let go
            args.manager.SelectExit(args.interactorObject, args.interactableObject);
            return;
        }

        isStickyActive = true;
        readyToRelease = false;

        if (returnProcess != null)
        {
            StopCoroutine(returnProcess);
            returnProcess = null;
        }

        if (bodyCollider != null)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), bodyCollider, true);
        }

        if (physicsBody != null)
        {
            physicsBody.isKinematic = isKinematicOnGrab;
            physicsBody.useGravity = false;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // If the user just let go of the button but didn't "click" to release, stay in hand
        if (!readyToRelease)
        {
            StartCoroutine(ReGrabRoutine(args.interactorObject));
            return; 
        }

        // Real release
        isStickyActive = false;

        if (bodyCollider != null)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), bodyCollider, false);
        }

        if (physicsBody != null)
        {
            physicsBody.isKinematic = false; 
            physicsBody.useGravity = useGravityOnRelease;
        }

        returnProcess = StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReGrabRoutine(IXRSelectInteractor interactor)
    {
        // Wait for the end of the frame to ensure XRI has finished the exit process
        yield return new WaitForEndOfFrame();
        
        if (interactor != null && grabComponent != null)
        {
            grabComponent.interactionManager.SelectEnter(interactor, (IXRSelectInteractable)grabComponent);
        }
    }

    public void PrepareToRelease()
    {
        readyToRelease = true;
    }

    IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(delayBeforeReturn);

        if (physicsBody != null)
        {
            physicsBody.linearVelocity = Vector3.zero;
            physicsBody.angularVelocity = Vector3.zero;
            physicsBody.isKinematic = isKinematicOnReturn;
        }

        transform.position = startingPosition;
        transform.rotation = startingRotation;
        
        returnProcess = null;
    }
}