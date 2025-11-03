using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; 
    [SerializeField] private GameObject pauseButtonUI;

    [Header("Input do Controle")]
    [SerializeField] private InputActionReference menuAction;

    [Header("Componentes do Jogador a Desativar")]
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private MonoBehaviour playerCameraScript; 
    [SerializeField] private MonoBehaviour[] playerInteractors;

    private bool isPaused = false;

    private void OnEnable()
    {
        menuAction.action.Enable();
        menuAction.action.performed += OnPauseButtonPressed;
    }

    private void OnDisable()
    {
        menuAction.action.Disable();
        menuAction.action.performed -= OnPauseButtonPressed;
    }

    void Start()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    private void OnPauseButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("<color=green>BOTÃO DE MENU PRESSIONADO!</color>");
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0; 

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerCameraScript != null) 
            playerCameraScript.enabled = false;

        foreach (var interactor in playerInteractors)
        {
            if (interactor != null)
                interactor.enabled = false;
        }
        
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(false);
        
        menuPanel.SetActive(true);
        Debug.Log("Jogo Pausado");
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (playerCameraScript != null)
            playerCameraScript.enabled = true;

        // Reativa todos os interactors
        foreach (var interactor in playerInteractors)
        {
            if (interactor != null)
                interactor.enabled = true;
        }

        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(true);

        menuPanel.SetActive(false);
        Debug.Log("Jogo Retomado");
    }
    
    public void ExitGame()
    {
        Debug.Log("Botão de Sair Clicado!");

        //No Editor do Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        
        // Se for uma build do jogo
        #else
        Application.Quit();
        #endif
    }
}
