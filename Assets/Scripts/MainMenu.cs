using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public GameObject loreContainer;
    public GameObject[] lores;
    public GameObject[] notKnown;

    public bool loreActive;
    public void startGame()
    {
        animator.SetTrigger("CanFadeOut");
        Invoke("loadScene", 2.5f);
        
    }

    void loadScene()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void loadLore()
    {

        if(loreActive)
        {
            loreContainer.SetActive(false);
            for (int i = 0; i < lores.Length; i++)
            {
                lores[i].SetActive(false);
                notKnown[i].SetActive(false);
            }
            loreActive = !loreActive;
            return;
        }

        loreContainer.SetActive(true);
        for (int i = 0; i < lores.Length; i++)
        {
            if (PlayerPrefs.HasKey("lore" + i))
                lores[i].SetActive(true);
            else
                notKnown[i].SetActive(true);
        }

        loreActive = !loreActive;
    }

    public void quit()
    {
        Application.Quit(0);
    }
}
