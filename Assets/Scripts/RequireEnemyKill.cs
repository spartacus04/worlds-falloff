using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireEnemyKill : MonoBehaviour
{
    public List<Enemy> enemies;
    public GameObject[] roomBlockers;
    private GameObject[] istances;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;


        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null) return;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        istances = new GameObject[roomBlockers.Length];

        if (!collision.gameObject.CompareTag("Player")) return;

        if (enemies.Count == 0)
        {
            Destroy(this);
            return;
        }

        for (int i = 0; i < roomBlockers.Length; i++)
        {
            istances[i] = Instantiate(roomBlockers[i], transform);
        }
    }
}
