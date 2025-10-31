using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip walkingSound;

    [Header("Volume e Transição")]
    [SerializeField] private float maxVolume = 0.8f;
    [SerializeField] private float fadeSpeed = 2.0f;

    [Header("Configurações de Corrida")]
    [SerializeField] private float walkPitch = 1.0f;
    [SerializeField] private float sprintPitch = 1.5f;

    [Header("Controlo de Habilidade")]
    public bool canSprint = false; // Começa como 'false'

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;

    private AudioSource walkingAudioSource;
    private bool isMoving = false;
    private bool isSprinting = false;


    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (sprintAction != null) sprintAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (sprintAction != null) sprintAction.action.Disable();
    }

    void Awake()
    {
        walkingAudioSource = GetComponent<AudioSource>();
        walkingAudioSource.clip = walkingSound;
        walkingAudioSource.loop = true;
        walkingAudioSource.playOnAwake = false;
        walkingAudioSource.volume = 0f;
    }

    void Update()
    {
        CheckMovement();

        if (isMoving)
        {
            ProcessFadeIn();
        }
        else
        {
            ProcessFadeOut();
        }

        // Ajusta o pitch do som com base no estado de corrida
        if (isSprinting)
        {
            walkingAudioSource.pitch = sprintPitch;
        }
        else
        {
            walkingAudioSource.pitch = walkPitch;
        }
    }

    void CheckMovement()
    {
        if (moveAction == null)
        {
            isMoving = false;
            return;
        }

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        isMoving = moveInput.magnitude > 0.1f;

        if (sprintAction != null)
        {
            // A corrida só é possível se o jogador PODE correr, se está a mover-se, E se está a pressionar o botão
            isSprinting = canSprint && isMoving && sprintAction.action.ReadValue<float>() > 0.5f;
        }
    }

    void ProcessFadeIn()
    {
        if (!walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }
        if (walkingAudioSource.volume < maxVolume)
        {
            walkingAudioSource.volume += fadeSpeed * Time.deltaTime;
            walkingAudioSource.volume = Mathf.Min(walkingAudioSource.volume, maxVolume);
        }
    }

    void ProcessFadeOut()
    {
        if (walkingAudioSource.volume > 0)
        {
            walkingAudioSource.volume -= fadeSpeed * Time.deltaTime;
            if (walkingAudioSource.volume <= 0.01f)
            {
                walkingAudioSource.volume = 0f;
                walkingAudioSource.Pause();
            }
        }
    }
}