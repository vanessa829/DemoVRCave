using UnityEngine;

public class CursorLock : MonoBehaviour
{
     void Start()
    {
        UpdateCursorState();
    }

    void Update()
    {
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}