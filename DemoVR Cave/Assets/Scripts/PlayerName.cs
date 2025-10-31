using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerName : MonoBehaviour
{
    public static PlayerName scene1;
    public TMP_InputField inputField;
    public string player_name;
    private void Awake()
    {
        if (scene1 == null)
        {
            scene1 = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetPlayerName()
    {
        player_name = inputField.text;
        SceneManager.LoadSceneAsync("Demo");
    }
}
