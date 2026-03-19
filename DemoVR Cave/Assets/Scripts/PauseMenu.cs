using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [Header("UI Panels")]
    public GameObject menuPanel; 
    
    [Header("Camera")]
    public Transform headTransform; 


    [Header("Current State")]
    public bool isPaused = false;
    public bool isPauseBlocked = false;

    private float lastToggleTime = 0f;
    private float toggleCooldown = 0.2f;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        Resume();
    }

    public void OnPauseButtonPressed(InputAction.CallbackContext context)
    {
        if (Time.unscaledTime - lastToggleTime < toggleCooldown) return;

        if (context.performed)
        {
            lastToggleTime = Time.unscaledTime;
            HandlePauseToggle();
        }
    }

    private void HandlePauseToggle()
    {
        if (isPauseBlocked) return;

        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }

    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}