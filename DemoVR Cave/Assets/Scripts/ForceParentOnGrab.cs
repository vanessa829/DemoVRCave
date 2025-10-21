using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ForceParentOnGrab : MonoBehaviour
{
    XRGrabInteractable grab;
    Transform originalParent;
    Vector3 savedLocalPos;
    Quaternion savedLocalRot;
    Vector3 savedLocalScale;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);

        originalParent = transform.parent;
        savedLocalPos = transform.localPosition;
        savedLocalRot = transform.localRotation;
        savedLocalScale = transform.localScale;
    }

    void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrab);
            grab.selectExited.RemoveListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // determine attach transform
        Transform attach = grab.attachTransform;
        if (attach == null && args.interactorObject != null)
        {
            var interactorMono = args.interactorObject as MonoBehaviour;
            if (interactorMono != null)
            {
                foreach (Transform t in interactorMono.transform)
                    if (t.name.ToLower().Contains("attach")) { attach = t; break; }
            }
        }

        Transform parentTo = attach != null ? attach : (args.interactorObject as MonoBehaviour)?.transform;
        if (parentTo == null)
        {
            Debug.LogWarning("[ForceParentOnGrabFixed] no parent found for attach");
            return;
        }

        // Log for debugging
        Debug.Log($"[ForceParentOnGrabFixed] Parented to {GetTransformPath(parentTo)} worldPos={parentTo.position}");

        // Parent while NOT preserving world position, so we can control local transform
        transform.SetParent(parentTo, worldPositionStays: false);

        // Reset local transform so the torch sits exactly at the attach point
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // If your parent has non-1 scale causing offset issues, normalise localScale (optional)
        transform.localScale = Vector3.one;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // restore parent and saved local pose/scale
        transform.SetParent(originalParent, worldPositionStays: false);
        transform.localPosition = savedLocalPos;
        transform.localRotation = savedLocalRot;
        transform.localScale = savedLocalScale;
    }

    string GetTransformPath(Transform t)
    {
        if (t == null) return "null";
        string path = t.name;
        Transform p = t.parent;
        while (p != null) { path = p.name + "/" + path; p = p.parent; }
        return path;
    }
}
