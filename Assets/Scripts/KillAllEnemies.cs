using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllEnemies : MonoBehaviour
{
    public List<Enemy> enemy;
    public GameObject unlockable;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            if (enemy[i] != null) return;
        }

        unlockable.SetActive(true);
        Destroy(gameObject);
    }
}
