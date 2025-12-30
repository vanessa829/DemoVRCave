using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
  
    [Header("PauseSettingsKey")]
    public KeyCode pauseKey = KeyCode.Escape;

    [Header("UI")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

     void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
