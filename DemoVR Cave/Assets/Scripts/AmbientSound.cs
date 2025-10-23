using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbienceSound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlayerInside = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null && boxCollider.isTrigger)
        {
            Debug.Log("✓ BoxCollider configurado como Trigger");
        }
        else
        {
            Debug.LogError("❌ BoxCollider precisa estar como Trigger!");
        }
    }

    void Start()
    {
        Debug.Log($"Script iniciado em: {gameObject.name}");
    }

    // Para Character Controller, use OnTriggerEnter/Exit normalmente
    // mas certifique-se que o XR Origin tem um Collider adicional
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {other.gameObject.name} (Tag: {other.tag})");
        
        // Verifica se é o player OU se é um filho do player (como a câmera)
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (!isPlayerInside)
            {
                Debug.Log("✓ Jogador ENTROU na área! Tocando som.");
                audioSource.Play();
                isPlayerInside = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"OnTriggerExit: {other.gameObject.name} (Tag: {other.tag})");
        
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (isPlayerInside)
            {
                Debug.Log("✓ Jogador SAIU da área! Parando som.");
                audioSource.Stop();
                isPlayerInside = false;
            }
        }
    }

    // Método alternativo para debug - mostra quando há overlap
    private void OnTriggerStay(Collider other)
    {
        if (!isPlayerInside)
        {
            Debug.Log($"OnTriggerStay: {other.gameObject.name} está dentro mas som não tocou ainda");
        }
    }
}