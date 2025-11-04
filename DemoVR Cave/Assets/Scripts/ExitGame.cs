using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void ExittheGame()
    {
        Debug.Log("Botão de Sair Clicado!");

        //No Editor do Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        
        // Se for uma build do jogo
        #else
        Application.Quit();
        #endif
    }
}
