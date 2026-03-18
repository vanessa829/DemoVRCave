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

    [Header("Sequência de Fim de Jogo")]
    public GameObject highscoreUIObject;
    public Credits creditsScript;

    [Header("Referências do Jogo")]
    public TimerCount timerScript; 

    private bool hasBeenTriggered = false;
    private Collider triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
        if (highscoreUIObject != null) highscoreUIObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (cutsceneTimeline != null) cutsceneTimeline.stopped += OnCutsceneFinished;
    }

    private void OnDisable()
    {
        if (cutsceneTimeline != null) cutsceneTimeline.stopped -= OnCutsceneFinished;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered || !other.transform.root.CompareTag("Player")) return;
        
        hasBeenTriggered = true;

        // Isto acontece quando o jogador entra no trigger.
        if (HighscoreManager.instance != null && timerScript != null)
        {
            // Para o cronómetro e obtém o tempo final
            float finalTime = timerScript.StopAndGetFinalTime();

            // Obtém o nome do jogador
            string playerName = PlayerName.scene1 != null ? PlayerName.scene1.playerName : "Convidado";

            // Manda o HighscoreManager adicionar e guardar a nova entrada
            HighscoreManager.instance.AddHighscoreEntry(playerName, finalTime);

            Debug.Log("Score guardado para " + playerName + " com o tempo de " + finalTime + " segundos.");
        }
        else
        {
            Debug.LogError("HighscoreManager ou TimerScript não foram encontrados! O score não foi guardado.");
        }

        Debug.Log("Jogador ativou a cutscene!");

        playerUIContainer.SetActive(false);
        foreach (AudioSource audio in playerSoundsToMute)
        {
            if (audio != null) audio.enabled = false;
        }
        playerMovementScript.enabled = false;
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
        cutsceneAudio.Play();
        if (cutsceneTimeline != null) cutsceneTimeline.Play();
        triggerCollider.enabled = false;
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("Cutscene terminada. A mostrar Highscores...");
        if (highscoreUIObject != null)
        {
            highscoreUIObject.SetActive(true);
        }
    }

    public void ShowCredits()
    {
        Debug.Log("Botão 'Next' pressionado. A iniciar os créditos...");
        if (highscoreUIObject != null)
        {
            highscoreUIObject.SetActive(false);
        }
        if (creditsScript != null)
        {
            creditsScript.StartCredits();
        }
    }
}