using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{

    [SerializeField] private int sceneIndex;

    public void ChangeScreen()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
