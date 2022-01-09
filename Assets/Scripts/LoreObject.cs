using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().loreCollected = true;
            Destroy(gameObject);
        }
    }
}
