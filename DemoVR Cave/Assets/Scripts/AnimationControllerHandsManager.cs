using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationHandsManager : MonoBehaviour
{
    public InputActionProperty triggerActionReference;
    public InputActionProperty gripActionReference;

    public Animator handAnimator;

    private void Awake()
    {
        

        SetupInputActions();
    }

    // to trigger the animation
    private void SetupInputActions()
    {
        if (triggerActionReference!=null && gripActionReference!=null)
        {
            triggerActionReference.action.performed += ctx => UpdateHandAnimation("Trigger", ctx.ReadValue<float>());
            triggerActionReference.action.canceled += ctx => UpdateHandAnimation("Trigger", 0);

            gripActionReference.action.performed += ctx => UpdateHandAnimation("Grip", ctx.ReadValue<float>());
            gripActionReference.action.canceled += ctx => UpdateHandAnimation("Grip", 0);
        }

        else
        {
            Debug.LogWarning("Input action references are not set in the inspector.");
        }
    }

    private void UpdateHandAnimation(string paramterName, float value)
    {
        if (handAnimator != null)
        {
            handAnimator.SetFloat(paramterName, value);
        }
        
    }

    //enable animation
    private void OnEnable()
    {
        triggerActionReference.action.Enable();
        gripActionReference.action.Enable();    
    }

    //disable animation
    private void OnDisable()
    {
        triggerActionReference.action.Disable();
        gripActionReference.action.Disable();   
    }

    
}
