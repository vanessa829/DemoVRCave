
using UnityEngine;
using TMPro; 
using System; 
using System.Collections.Generic; 

public class HighscoreUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreTimeText;
    public TextMeshProUGUI scorePositionText;

    void Start()
    {
        if (HighscoreManager.instance == null)
        {
            Debug.LogError("HighscoreManager não foi encontrado na cena! Certifique-se que ele existe e persiste desde a primeira cena.");
            return;
        }

        // Pega o nome do jogador que veio da cena 1
        string currentPlayerName = "Convidado"; 
        if (PlayerName.scene1 != null)
        {
            currentPlayerName = PlayerName.scene1.player_name;
        }

        // Pede a lista completa e ordenada de recordes
        List<HighscoreEntry> highscoreList = HighscoreManager.instance.GetHighscoreList();

       
        
        int playerIndex = highscoreList.FindIndex(entry => entry.playerName == currentPlayerName);

        // Se encontrámos o jogador na lista
        if (playerIndex != -1)
        {
            // Pega a entrada específica do jogador
            HighscoreEntry playerEntry = highscoreList[playerIndex];

            // Preenche os campos de texto da UI
            nameText.text = playerEntry.playerName;
            scorePositionText.text = (playerIndex + 1).ToString(); // Posição é o índice + 1

            // Formata o tempo de segundos para o formato MM:SS.FF
            TimeSpan time = TimeSpan.FromSeconds(playerEntry.time);
            scoreTimeText.text = time.ToString(@"mm\:ss");
        }
        else
        {
            // Se o jogador não estiver na lista (ex: primeira vez a jogar)
            nameText.text = currentPlayerName;
            scoreTimeText.text = "--:--";
            scorePositionText.text = "N/A";
            Debug.LogWarning("O jogador '" + currentPlayerName + "' não foi encontrado na tabela de recordes.");
        }
    }
}