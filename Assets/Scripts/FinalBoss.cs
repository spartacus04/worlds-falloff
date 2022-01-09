using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalBoss : MonoBehaviour
{
    public GameObject player;
    public GameObject[] tentacles;
    public GameObject[] eyes;
    public GameObject tentacleContainer;
    public LookAt LookAt;
    public GameObject enemy;
    public GameObject bomberEnemy;
    public Transform location;
    public Color damageColor;
    public int health = 3;
    public int last;
    public TextMeshProUGUI text;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Invoke("bossLoop", 5f);
    }

    // Update is called once per frame
    void bossLoop()
    {
        if (health <= 0) return;

        LookAt.Senabled = false;
        last = last == 3 ? Random.Range(1, 3) : Random.Range(1, 4);
        switch (last)
        {
            case 1:
                attack1();
                break;
            case 2:
                attack2();
                break;
            case 3:
                attack3();
                break;
        }
    }

    void attack1()
    {
        animator.SetTrigger("RotateRandom");
        Invoke("attack1_2", Random.Range(2f, 6f));
    }

    void attack1_2()
    {
        animator.SetTrigger("TentaclesOut");
        Invoke("bossLoop", 5f);
    }

    void attack2()
    {
        animator.SetTrigger("OpenMouth");
        GameObject iEnemy = Instantiate(enemy, location);
        iEnemy.GetComponent<Enemy>().enemyEnabled = true;
        Invoke("bossLoop", 10f);
    }

    void attack3()
    {
        animator.SetTrigger("OpenMouth");
        GameObject iEnemy = Instantiate(bomberEnemy, location);
        iEnemy.GetComponent<BomberEnemy>().enemyEnabled = true;
        Invoke("bossLoop", 3f);
    }

    public void damage()
    {
        --health;
        GetComponent<SpriteRenderer>().color = damageColor;
        if (health <= 0)
        {
            animator.SetTrigger("BossDeath");

            if(player.GetComponent<PlayerStats>().loreCollected)
            {
                int i = 0;
                do
                {
                    if (shouldSetKey(i))
                    {
                        PlayerPrefs.SetString("lore" + i, "haha yes");
                        break;
                    }
                    else i++;
                } while (i < 3);
            }

            StartCoroutine(youWin());
            Invoke("loadScene", 6f);
        }
        Invoke("normal", 1f);
    }

    public void normal()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void loadScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public IEnumerator youWin()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(2f);
        player.GetComponent<PlayerStats>().animator.SetTrigger("CanFadeOut");

        yield return new WaitForSeconds(2.5f);

        float currentTime = Time.time + 0.5f;
        for(float t = 0; t < currentTime; t += Time.deltaTime)
        {
            text.color = new Color(255, 255, 255, Mathf.Clamp(t / 0.5f, 0, 1));
            yield return 0;
        }

        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene("MainMenuScene");
    }

    public bool shouldSetKey(int n)
    {
        return !PlayerPrefs.HasKey("lore" + n);
    }
}
