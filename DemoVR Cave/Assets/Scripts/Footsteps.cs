using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("O som de caminhada contínuo. Deve ser um loop.")]
    [SerializeField] private AudioClip walkingSound;

    [Header("Volume e Transição")]
    [Tooltip("O volume máximo que o som de caminhada deve atingir.")]
    [SerializeField] private float maxVolume = 0.8f;
    [Tooltip("A velocidade com que o som aparece (fade in) e desaparece (fade out).")]
    [SerializeField] private float fadeSpeed = 2.0f;

    // --- NOVAS ADIÇÕES PARA A CORRIDA ---
    [Header("Configurações de Corrida")]
    [Tooltip("O pitch (tom) do som quando está a andar normalmente.")]
    [SerializeField] private float walkPitch = 1.0f;
    [Tooltip("O pitch (tom) do som quando está a correr. Um valor mais alto soa mais rápido.")]
    [SerializeField] private float sprintPitch = 1.5f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction; // <-- NOVO

    private AudioSource walkingAudioSource;
    private bool isMoving = false;
    private bool isSprinting = false; // <-- NOVO

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
        if (sprintAction != null) // <-- NOVO
            sprintAction.action.Enable(); // <-- NOVO
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
        if (sprintAction != null) // <-- NOVO
            sprintAction.action.Disable(); // <-- NOVO
    }

    void Awake()
    {
        // Configura o AudioSource principal
        walkingAudioSource = GetComponent<AudioSource>();
        walkingAudioSource.clip = walkingSound;
        walkingAudioSource.loop = true; // O som deve estar em loop
        walkingAudioSource.playOnAwake = false;
        walkingAudioSource.volume = 0f; // Começa em silêncio
    }

    void Update()
    {
        CheckMovement();

        if (isMoving)
        {
            // Se está a mover-se, faz fade in do som
            ProcessFadeIn();
        }
        else
        {
            // Se está parado, faz fade out do som
            ProcessFadeOut();
        }

        // --- NOVA LÓGICA PARA O PITCH ---
        // Ajusta o pitch do som com base no estado de corrida
        if (isSprinting)
        {
            walkingAudioSource.pitch = sprintPitch;
        }
        else
        {
            walkingAudioSource.pitch = walkPitch;
        }
        // ---------------------------------
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

        // --- NOVA LÓGICA PARA DETETAR A CORRIDA ---
        if (sprintAction != null)
        {
            // A corrida só é possível se o jogador já se estiver a mover
            isSprinting = isMoving && sprintAction.action.ReadValue<float>() > 0.5f;
        }
        // -----------------------------------------
    }

    void ProcessFadeIn()
    {
        // Se o som não estiver a tocar, começa a tocar
        if (!walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }

        // Aumenta o volume gradualmente até ao máximo
        if (walkingAudioSource.volume < maxVolume)
        {
            walkingAudioSource.volume += fadeSpeed * Time.deltaTime;
            walkingAudioSource.volume = Mathf.Min(walkingAudioSource.volume, maxVolume);
        }
    }

    void ProcessFadeOut()
    {
        // Diminui o volume gradualmente até zero
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