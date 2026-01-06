using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    public float zoomFOV = 30f;
    public float speed = 5f;

    private bool zoom;

    // Update is called once per frame
    void Update()
    {

        if (!zoom) return;

        Camera cam = GetComponent<Camera>();
        cam.fieldOfView = Mathf.Lerp(
            cam.fieldOfView,
            zoomFOV,
            Time.deltaTime * speed
        );

    }

    public void StartZoom()
    {
        zoom = true;
    }
}
