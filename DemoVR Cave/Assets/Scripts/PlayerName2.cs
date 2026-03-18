using TMPro;
using UnityEngine;

public class PlayerName2 : MonoBehaviour
{
    public TextMeshProUGUI displayPlayerName;

    private void Awake()
    {
        if (displayPlayerName == null) return;

        if (PlayerName.instance != null)
        {
            displayPlayerName.text = PlayerName.instance.playerName;
        }
        else
        {
            // Default text for testing directly in the Demo scene
            displayPlayerName.text = "Guest Player";
            Debug.Log("PlayerName2: No PlayerName instance found. Using default name.");
        }
    }
}