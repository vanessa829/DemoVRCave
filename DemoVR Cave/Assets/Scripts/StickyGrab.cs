using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]

public class StickyGrab : MonoBehaviour
{
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    // Quando o jogador tenta soltar...
    private void OnRelease(SelectExitEventArgs args)
    {
        // ...força o interactor (a mão) a agarrar o objeto novamente no mesmo frame.
        args.interactorObject.transform.GetComponent<XRBaseInteractor>().StartManualInteraction(GetComponent<IXRSelectInteractable>());
    }
}
