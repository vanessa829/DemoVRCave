using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerName2 : MonoBehaviour
{
    public TextMeshProUGUI display_player_name;
    private void Awake()
    {

        display_player_name.text = PlayerName.scene1.player_name;
    }

}
