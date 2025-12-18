using UnityEngine;
using UnityEngine.SceneManagement;

public class BunnaUI : MonoBehaviour
{
    public Animator animator;
    public string sceneName;

    public void OnClickStart()
    {

        animator.SetTrigger("StartWindow");

    }

    public void ChangeScreen()
    {
        SceneManager.LoadScene(sceneName);
    }
}


