using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseMenu;
    public AudioSource mainTheme;
    public float reducedVolume;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            if (isPaused)
            {
                Time.timeScale = 0;
                mainTheme.volume = reducedVolume;
            }
            else
            {
                Time.timeScale = 1;
                mainTheme.volume = 1;
            }
        }       
    }

    public void resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        mainTheme.volume = 1;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        mainTheme.volume = 1;
    }

    public void quit()
    {
        Application.Quit(0);
    }
}
