using UnityEngine;

public class Credits : MonoBehaviour
{
[Header("Referências Principais")]
    public GameObject creditsCanvasObject; 
    public GameObject creditsContentObject; 

    [Header("Configurações do Scroll")]
    public float scrollSpeed = 10f;
    public float startPositionY = -500f;

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

    
    public void StartCredits()
    {
        Debug.Log("A iniciar os créditos por chamada externa!");

        if (creditsCanvasObject == null || creditsContentObject == null)
        {
            Debug.LogError("ERRO: O 'CanvasCredits' ou o 'ScrollContent' não foram atribuídos no Inspector!");
            return;
        }

        // Ativa o canvas principal
        creditsCanvasObject.SetActive(true);

        // Define a posição inicial do conteúdo
        creditsRectTransform.anchoredPosition = new Vector2(creditsRectTransform.anchoredPosition.x, startPositionY);

        // Ativa o conteúdo (por segurança)
        creditsContentObject.SetActive(true);

        // Libera o scroll para começar no Update()
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
