using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{

    [SerializeField] private int sceneIndex;

    public static string previousScene;


    public void ChangeScreen()
    {
        SceneManager.LoadScene(sceneIndex);


    }


    public static void LoadSettings()
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Settings");
    }

    public void GoBack()
    {
        SceneManager.LoadScene(previousScene);
    }
}
