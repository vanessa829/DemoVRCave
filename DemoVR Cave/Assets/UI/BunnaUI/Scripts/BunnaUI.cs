using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BunnaUI : MonoBehaviour
{

    public string sceneName;
   

    public void OnClickStart()
    {

        



    }

    public void ChangeScreen()
    {
        SceneManager.LoadScene(sceneName);
    }

  
}


