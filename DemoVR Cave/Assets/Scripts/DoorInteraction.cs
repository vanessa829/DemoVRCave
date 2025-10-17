using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    // Arraste aqui no Inspector o objeto que tem o componente Animator 
    [Tooltip("O Animator que controla a animacão da janela.")]
    public Animator windowAnimator;

    // O nome do parâmetro Trigger no Animator Controller para ABRIR
    [Tooltip("O nome exato do Trigger no Animator para abrir a janela.")]
    public string openTriggerName = "Open";

    // --- NOVO ---
    // O nome do parâmetro Trigger no Animator Controller para FECHAR
    [Tooltip("O nome exato do Trigger no Animator para fechar a janela.")]
    public string closeTriggerName = "Close";

    // --- NOVO ---
    // Variável para guardar o estado da janela (false = fechada, true = aberta)
    private bool isWindowOpen = false;

    // Renomeamos a função para ser mais clara. Agora ela alterna o estado.
    public void ToggleWindowState()
    {
        // Verificamos se a referência do Animator foi definida no Inspector.
        if (windowAnimator == null)
        {
            Debug.LogError("ERRO: O Animator não foi atribuído no script WindowInteraction!", this.gameObject);
            return; // Sai da função se o animator não existir.
        }

        
        // Se a janela NÃO estiver aberta...
        if (isWindowOpen == false)
        {
            // ...então acionamos a animação de ABRIR.
            Debug.Log("Acionando trigger para ABRIR: " + openTriggerName);
            windowAnimator.SetTrigger(openTriggerName);
            // E atualizamos o estado para dizer que a janela agora está ABERTA.
            isWindowOpen = true;
        }
        else // Se a janela JÁ estiver aberta...
        {
            // ...então acionamos a animação de FECHAR.
            Debug.Log("Acionando trigger para FECHAR: " + closeTriggerName);
            windowAnimator.SetTrigger(closeTriggerName);
            // E atualizamos o estado para dizer que a janela agora está FECHADA.
            isWindowOpen = false;
        }
    }
}
