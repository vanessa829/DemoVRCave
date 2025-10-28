using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class CinematicTrigger : MonoBehaviour
{
    [Header("Referências da Cutscene")]
    [Tooltip("O componente de script que controla o movimento do jogador (ex: PhysicsBasedLocomotion).")]
    public MonoBehaviour playerMovementScript;

    [Tooltip("A câmara principal do jogador (a que está dentro do XR Origin).")]
    public Camera playerCamera;

    [Tooltip("A segunda câmara, a da cutscene, que vai ser ativada.")]
    public Camera cutsceneCamera;

    [Tooltip("O AudioSource com a música da cutscene.")]
    public AudioSource cutsceneAudio;

    [Tooltip("(Opcional) Arraste o Playable Director se quiser iniciar uma Timeline também.")]
    public PlayableDirector cutsceneTimeline;

    // --- NOVA ADIÇÃO ---
    [Header("Sons a Desativar")]
    [Tooltip("Arraste para aqui todos os AudioSources do jogador que devem ser silenciados (passos, respiração, etc.).")]
    public AudioSource[] playerSoundsToMute;


    private bool hasBeenTriggered = false;
    private Collider triggerCollider; // Referência para o nosso próprio colisor

    void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered || !other.transform.root.CompareTag("Player"))
        {
            return;
        }

        if (playerMovementScript == null || playerCamera == null || cutsceneCamera == null || cutsceneAudio == null)
        {
            Debug.LogError("ERRO: Uma ou mais referências não foram atribuídas no CinematicTrigger!", this.gameObject);
            return;
        }

        Debug.Log("Jogador ativou a cutscene!");
        hasBeenTriggered = true;


        // 1. Silencia os sons do jogador
        foreach (AudioSource audio in playerSoundsToMute)
        {
            if (audio != null)
            {
                audio.Stop(); // Ou audio.enabled = false;
                audio.enabled = false;
            }
        }

        // 2. Desativa o controlo de movimento do jogador.
        playerMovementScript.enabled = false;

        // 3. Desativa a câmara principal do jogador.
        playerCamera.gameObject.SetActive(false);

        // 4. Ativa a câmara da cutscene (e o seu Audio Listener, se o tiver).
        cutsceneCamera.gameObject.SetActive(true);

        // 5. Toca a música da cutscene.
        if (!cutsceneAudio.gameObject.activeInHierarchy)
        {
            cutsceneAudio.gameObject.SetActive(true);
        }
        cutsceneAudio.Play();

        // 6. (Opcional) Inicia a Timeline.
        if (cutsceneTimeline != null)
        {
            cutsceneTimeline.Play();
        }

        triggerCollider.enabled = false;
        Debug.Log("Trigger desativado para não ser chamado novamente.");
    }
}