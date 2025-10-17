using UnityEngine;

// Garante que este objeto tenha sempre um AudioSource
[RequireComponent(typeof(AudioSource))]
public class AmbienceSound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayerEntered = false; // Para garantir que o som só toca uma vez

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Garante que o som não toca ao iniciar a cena
        audioSource.playOnAwake = false;
    }

    // Esta função é chamada AUTOMATICAMENTE pelo Unity quando
    // um outro collider (marcado como trigger) entra neste.
    private void OnTriggerEnter(Collider other)
    {
        
        // Verifica se o objeto que entrou tem a tag "Player" E se o som ainda não tocou
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            Debug.Log("Jogador entrou na área! A tocar o som.");

            // Toca o som
            audioSource.Play();

            // Marca que o jogador já entrou para não repetir o som
            hasPlayerEntered = true;
        }
    }

    public void Start()
    {
        Debug.Log("Script iniciado");
    }

    // (Opcional) Se quiser que o som pare quando o jogador sai
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador saiu da área! A parar o som.");
            audioSource.Stop();
            hasPlayerEntered = false; // Permite que o som toque novamente se o jogador reentrar
        }
    }
}
