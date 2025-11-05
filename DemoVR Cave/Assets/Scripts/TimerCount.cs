using UnityEngine;
using TMPro;
using System; 
public class TimerCount : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime;

    private bool isTiming = true;

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            
            // Usar TimeSpan é um pouco mais limpo para formatar
            TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
            timerText.text = time.ToString(@"mm\:ss");
        }
    }

    public float StopAndGetFinalTime()
    {
        // 1. Desliga o interruptor
        isTiming = false;
        Debug.Log("Cronómetro parado com o tempo final de: " + elapsedTime + " segundos.");

        // 2. Devolve o tempo final para quem perguntou
        return elapsedTime;
    }
}
