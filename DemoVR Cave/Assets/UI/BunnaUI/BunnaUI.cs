using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BunnaUI : MonoBehaviour
{
    public Animator animator;
    public string sceneName;
    public GameObject windowClosed;
    public GameObject windowOpen;


    public void OnClickStart()
    {



        windowClosed.SetActive(false);
        windowOpen.SetActive(true);

        Animator anim = windowOpen.GetComponent<Animator>();
        anim.Rebind();              
        anim.Update(0f);
        anim.SetTrigger("StartWindow");
        



    }

    public void ChangeScreen()
    {
        SceneManager.LoadScene(sceneName);
    }

  
}


