using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Referências das Portas")]
    [Tooltip("Arraste o Animator da porta esquerda para aqui.")]
    public Animator leftDoorAnimator;
    [Tooltip("Arraste o Animator da porta direita para aqui.")]
    public Animator rightDoorAnimator;

    [Header("Referências de Áudio")]
    [Tooltip("Arraste o AudioSource que tem o som da porta a abrir.")]
    public AudioSource doorAudioSource;

    [Header("Nome da Animação")]
    [Tooltip("O nome do Trigger no Animator para ABRIR as portas.")]
    public string openTriggerName = "Open";

    // Variável de controlo para garantir que a ação só acontece uma vez.
    private bool haveDoorsBeenOpened = false;

    // Esta é a função pública que será chamada pela interação (Near/Far Interactor)
    public void OpenDoorsOnce()
    {
        
        // Se as portas já foram abertas, a função termina imediatamente e não faz mais nada.
        if (haveDoorsBeenOpened)
        {
            Debug.Log("As portas já foram abertas. Nenhuma ação será executada.");
            return;
        }

        // Verifica se todas as referências foram atribuídas no Inspector
        if (leftDoorAnimator == null || rightDoorAnimator == null || doorAudioSource == null)
        {
            Debug.LogError("ERRO: Uma ou mais referências (Animators ou AudioSource) não foram atribuídas no CoordinatedDoorController!", this.gameObject);
            return;
        }

        Debug.Log("Primeira interação! A abrir ambas as portas e a tocar o som.");

        // Aciona a animação de ABRIR em ambas as portas.
        leftDoorAnimator.SetTrigger(openTriggerName);
        rightDoorAnimator.SetTrigger(openTriggerName);

        // Toca o som da porta.
        doorAudioSource.Play();

        
        haveDoorsBeenOpened = true;
    }
}
