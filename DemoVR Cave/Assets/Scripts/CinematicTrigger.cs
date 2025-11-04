using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class CinematicTrigger : MonoBehaviour
{
    [Header("Referências da Cutscene")]
    public MonoBehaviour playerMovementScript;
    public Camera playerCamera;
    public Camera cutsceneCamera;
    public AudioSource cutsceneAudio;
    public PlayableDirector cutsceneTimeline;

    [Header("Sons a Desativar")]
    public AudioSource[] playerSoundsToMute;

    [Header("UI a Desativar")]
    public GameObject playerUIContainer;

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

        // Desativa toda a UI do jogador.
        playerUIContainer.SetActive(false);
        Debug.Log("UI do jogador desativada.");

        // Silencia os sons do jogador
        foreach (AudioSource audio in playerSoundsToMute)
        {
            if (audio != null)
            {
                audio.Stop(); // Ou audio.enabled = false;
                audio.enabled = false;
            }
        }

        // Desativa o controlo de movimento do jogador.
        playerMovementScript.enabled = false;

        // Desativa a câmara principal do jogador.
        playerCamera.gameObject.SetActive(false);

        // Ativa a câmara da cutscene (e o seu Audio Listener, se o tiver).
        cutsceneCamera.gameObject.SetActive(true);

        // Toca a música da cutscene.
        if (!cutsceneAudio.gameObject.activeInHierarchy)
        {
            cutsceneAudio.gameObject.SetActive(true);
        }
        cutsceneAudio.Play();

        // Inicia a Timeline.
        if (cutsceneTimeline != null)
        {
            cutsceneTimeline.Play();
        }

        triggerCollider.enabled = false;
        Debug.Log("Trigger desativado para não ser chamado novamente.");
    }
}