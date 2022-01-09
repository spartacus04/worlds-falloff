using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Enemy> enemyList;

    public List<BomberEnemy> bomberEnemyList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyList != null)
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i] != null)
                        enemyList[i].enemyEnabled = true;
                }

            if (bomberEnemyList != null)
                for (int i = 0; i < bomberEnemyList.Count; i++)
                {
                    if (bomberEnemyList[i] != null)
                        bomberEnemyList[i].enemyEnabled = true;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyList != null)
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i] != null)
                        enemyList[i].enemyEnabled = false;
                }

            if (bomberEnemyList != null)
                for (int i = 0; i < bomberEnemyList.Count; i++)
                {
                    if (bomberEnemyList[i] != null)
                    {
                        bomberEnemyList[i].enemyEnabled = false;
                        bomberEnemyList[i].transform.position = bomberEnemyList[i].startingPos;
                        bomberEnemyList[i].spriteRenderer.sprite = bomberEnemyList[i].frames[0];
                    }
                }
        }
    }
}