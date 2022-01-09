using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    public List<GunGeneric> guns;
    public bool generateRandom;
    // Start is called before the first frame update
    SpriteRenderer spriteRenderer;
    public GunGeneric gun;
    public float pickupTime;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(generateRandom)
        {
            generateRandomWeapon();
        }

        if(gun != null)
            renderSprite();
    }

    public void generateRandomWeapon()
    {
        gun = guns[Random.Range(0, guns.Count)];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>() != null && gun != null)
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();

            if(player.addGun(Instantiate(gun)))
            {
                Destroy(gameObject);
            }
        }
    }

    public void renderSprite()
    {
        spriteRenderer.sprite = gun.artwork;
    }

    public void setGun(GunGeneric gun, float time = 0)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.gun = gun;
        pickupTime = time;
        renderSprite();
    }
}
