using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class QuickAttachInspector : MonoBehaviour
{
    XRGrabInteractable grab;
    void Awake() => grab = GetComponent<XRGrabInteractable>();

    void Update()
    {
        if (grab.isSelected)
        {
            Transform a = grab.attachTransform;
            string attachName = a ? a.name + " (" + GetTransformPath(a) + ")" : "null";
            var interactor = grab.firstInteractorSelecting as MonoBehaviour;
            string interactorPath = interactor ? GetTransformPath(interactor.transform) : "null";
            Debug.Log($"[ATTACH_DEBUG] attach={attachName} interactor={interactorPath} torchParent={transform.parent?.name ?? "null"} torchPos={transform.position:F3}");
        }
    }

    string GetTransformPath(Transform t)
    {
        string path = t.name;
        Transform p = t.parent;
        while (p != null) { path = p.name + "/" + path; p = p.parent; }
        return path;
    }
}
