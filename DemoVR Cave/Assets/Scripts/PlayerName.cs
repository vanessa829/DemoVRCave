using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerName : MonoBehaviour
{
    public static PlayerName instance;
    public static PlayerName scene1;
    public TMP_InputField inputField;
    public string playerName;
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
        playerName = inputField.text;
        SceneManager.LoadSceneAsync("Demo");
    }
}
