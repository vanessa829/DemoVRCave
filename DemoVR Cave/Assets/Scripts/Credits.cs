using UnityEngine;
using UnityEngine.Playables; 

public class Credits : MonoBehaviour
{
    [Header("Referências Principais")]
    public GameObject creditsCanvasObject; 
    public GameObject creditsContentObject; // O objeto que vai fazer o scroll

    [Header("Configurações do Scroll")]
    public float scrollSpeed = 10f;
    public float startPositionY = -500f;

    [Header("Referências da Cutscene")]
    public PlayableDirector cutsceneTimeline;
    public float delayInSecondsAfterCutscene = 1f;

    private RectTransform creditsRectTransform;
    private bool startScrolling = false;

    void Awake()
    {
        if (creditsContentObject != null)
        {
            creditsRectTransform = creditsContentObject.GetComponent<RectTransform>();
        }

        if (creditsCanvasObject != null)
        {
            creditsCanvasObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (cutsceneTimeline != null)
        {
            cutsceneTimeline.stopped += OnCutsceneFinished;
        }
    }

    private void OnDisable()
    {
        if (cutsceneTimeline != null)
        {
            cutsceneTimeline.stopped -= OnCutsceneFinished;
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("A cutscene terminou. A iniciar a contagem para os créditos...");
        Invoke("StartCredits", delayInSecondsAfterCutscene);
    }

    private void StartCredits()
    {
        Debug.Log("A iniciar os créditos!");

        // Verifica se as referências estão atribuídas para evitar erros.
        if (creditsCanvasObject == null || creditsContentObject == null)
        {
            Debug.LogError("ERRO: O 'CanvasCredits' ou o 'ScrollContent' não foram atribuídos no Inspector!");
            return;
        }


        creditsCanvasObject.SetActive(true);

        creditsRectTransform.anchoredPosition = new Vector2(creditsRectTransform.anchoredPosition.x, startPositionY);

        creditsContentObject.SetActive(true);

        startScrolling = true;
    }

    void Update()
    {
        if (startScrolling && creditsRectTransform != null)
        {
            creditsRectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }
}
