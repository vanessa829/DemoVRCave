using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadinScreen : MonoBehaviour
{
    [Header("Load Scene")]
    [SerializeField] private string nextSceneName;

    [Header("Settings Value")]
    [SerializeField] private float minimumLoadTime = 0.5f;

    private void Start()
    {
        StartCoroutine(LoadScene());

    }

    IEnumerator LoadScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(nextSceneName);
        load.allowSceneActivation = false;

        //wait until game is fully loaded
        while (load.progress < 0.9f)
        {
            yield return null;
        }


        //wait a lil more
        yield return new WaitForSeconds(minimumLoadTime);

        load.allowSceneActivation = true;
    }


}
