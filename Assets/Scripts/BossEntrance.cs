using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    public GameObject room;

    private void Start()
    {
        Object.DontDestroyOnLoad(room);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Movement>().canMove = false;
            playFadeOut(collision.gameObject.GetComponent<PlayerStats>().animator);
            StartCoroutine(switchScene(collision.gameObject.GetComponent<PlayerStats>().guns, collision.gameObject.GetComponent<PlayerStats>().selectedInventorySlot, collision.gameObject.GetComponent<PlayerStats>().loreCollected));
        }
    }

    void playFadeOut(Animator animator)
    {
        animator.SetTrigger("CanFadeOut");
    }

    IEnumerator switchScene(GunGeneric[] guns, int slot, bool sborra)
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("FinalBossScene");
        yield return new WaitForFixedUpdate();

        PlayerStats newPlayer = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();

        for (int i = 0; i < 3; i++)
        {
            newPlayer.guns[i] = guns[i];
        }

        newPlayer.selectedInventorySlot = slot;
        newPlayer.loreCollected = sborra;
        newPlayer.renderMenu();

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(room);
    }
}
