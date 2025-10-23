using UnityEngine;
using UnityEngine.InputSystem;

public class Footsteps : MonoBehaviour
{
     [Header("Audio")]
    [SerializeField] private AudioClip[] footstepSounds;
    
    [Header("Timing")]
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float runStepInterval = 0.3f;
    
    [Header("Volume")]
    [SerializeField] private float walkVolume = 0.6f;
    [SerializeField] private float runVolume = 0.8f;
    
    [Header("Fade Out")]
    [SerializeField] private bool useFadeOut = true;
    [SerializeField] private float fadeOutSpeed = 5f;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;
    
    private AudioSource currentStepSource;
    private AudioSource[] stepSources; // Pool de AudioSources
    private int currentSourceIndex = 0;
    
    private float stepTimer = 0f;
    private bool isMoving = false;
    private bool wasMoving = false;
    private bool isRunning = false;
    private bool isFadingOut = false;

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
        if (sprintAction != null)
            sprintAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
        if (sprintAction != null)
            sprintAction.action.Disable();
    }

    void Awake()
    {
        // Cria pool de 2 AudioSources (para overlap de sons)
        stepSources = new AudioSource[2];
        
        for (int i = 0; i < stepSources.Length; i++)
        {
            GameObject sourceObj = new GameObject($"FootstepSource_{i}");
            sourceObj.transform.SetParent(transform);
            
            AudioSource source = sourceObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f;
            
            stepSources[i] = source;
        }
        
        currentStepSource = stepSources[0];
    }

    void Update()
    {
        CheckMovement();
        
        // Detecta quando para de mover
        if (wasMoving && !isMoving)
        {
            OnStopMoving();
        }
        // Detecta quando começa a mover (cancela fade out)
        else if (!wasMoving && isMoving)
        {
            OnStartMoving();
        }
        
        if (isMoving && !isFadingOut)
        {
            ProcessFootsteps();
        }
        else if (isFadingOut)
        {
            ProcessFadeOut();
        }
        else
        {
            stepTimer = 0f;
        }
        
        wasMoving = isMoving;
    }

    void CheckMovement()
    {
        if (moveAction == null)
        {
            isMoving = false;
            return;
        }
        
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        
        if (Mathf.Abs(moveInput.x) < 0.1f && Mathf.Abs(moveInput.y) < 0.1f)
        {
            moveInput = Vector2.zero;
        }
        
        isMoving = moveInput.magnitude > 0.3f;
        
        if (sprintAction != null)
        {
            isRunning = sprintAction.action.ReadValue<float>() > 0.5f && isMoving;
        }
    }

    void ProcessFootsteps()
    {
        if (footstepSounds.Length == 0) return;
        
        stepTimer += Time.deltaTime;
        float currentInterval = isRunning ? runStepInterval : walkStepInterval;
        
        if (stepTimer >= currentInterval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        // Alterna entre AudioSources do pool
        currentSourceIndex = (currentSourceIndex + 1) % stepSources.Length;
        currentStepSource = stepSources[currentSourceIndex];
        
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        currentStepSource.pitch = Random.Range(0.9f, 1.1f);
        float volume = isRunning ? runVolume : walkVolume;
        
        currentStepSource.PlayOneShot(clip, volume);
        
        Debug.Log($"Passo tocado! Source: {currentSourceIndex}");
    }

    void OnStopMoving()
    {
        Debug.Log("Parou de mover!");
        
        if (useFadeOut)
        {
            // Inicia fade out suave
            isFadingOut = true;
        }
        else
        {
            // Para imediatamente
            StopAllFootsteps();
        }
    }

    void OnStartMoving()
    {
        Debug.Log("Começou a mover!");
        isFadingOut = false;
        
        // Restaura volumes
        foreach (var source in stepSources)
        {
            source.volume = 1f;
        }
    }

    void ProcessFadeOut()
    {
        bool anyPlaying = false;
        
        foreach (var source in stepSources)
        {
            if (source.isPlaying)
            {
                // Fade out gradual
                source.volume -= fadeOutSpeed * Time.deltaTime;
                
                if (source.volume <= 0.01f)
                {
                    source.Stop();
                    source.volume = 1f; // Restaura para próximo uso
                }
                else
                {
                    anyPlaying = true;
                }
            }
        }
        
        // Se nenhum som está tocando, termina o fade out
        if (!anyPlaying)
        {
            isFadingOut = false;
        }
    }

    void StopAllFootsteps()
    {
        foreach (var source in stepSources)
        {
            source.Stop();
            source.volume = 1f;
        }
        
        isFadingOut = false;
    }
}