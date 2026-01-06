using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    public CameraZoomIn cameraZoom;


    public void OnUIAnimationFinished()
    {
        cameraZoom.StartZoom();
    }
}
