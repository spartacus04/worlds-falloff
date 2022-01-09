using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    public void startGame()
    {
        animator.SetTrigger("CanFadeOut");
        Invoke("loadGameScene", 2.5f);
    }

    void loadGameScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void mainMenu()
    {
        animator.SetTrigger("CanFadeOut");
        Invoke("loadMenuScene", 2.5f);
    }

    void loadMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }
}
