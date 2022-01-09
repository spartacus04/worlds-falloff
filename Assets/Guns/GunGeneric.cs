using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new gun", menuName = "Gun")]
public class GunGeneric : ScriptableObject
{
    public new string name;
    public Sprite artwork;
    public Sprite weaponInHand;
    public GameObject bullet;
    public float bulletSpeed;
    public int damagePerBullet;
    public float reloadTime;
    public int bulletsPerCharger;
    public int bullets;
    public AudioClip shootSound;
    [HideInInspector]
    public bool isReloading;

    public float fireRate = 0.5f;
    public float nextFire = 0;

    public virtual void shoot(Transform shootPoint, GameObject self, string target, Vector2 mousePos)
    {
        PlayerStats player;
        if (shootPoint.parent != null)
            player = shootPoint.parent.gameObject.GetComponent<PlayerStats>();
        else
            player = null;

        if (Time.time > nextFire)
        {
            if(bullets == 0)
            {
                nextFire = Time.time + reloadTime;
                isReloading = true;
                
                bullets = bulletsPerCharger;
                return;
            } else isReloading = false;
            
            --bullets;
            if(player != null)
            {
                player.ammoText.text = bullets.ToString();
                float percent = (float)bullets / (float)bulletsPerCharger;

                player.ammoTransform.sizeDelta = new Vector2(player.ammoTransform.sizeDelta.x, 64 * percent);
            }

            if(shootSound != null)
                AudioSource.PlayClipAtPoint(shootSound, shootPoint.position);

            nextFire = Time.time + fireRate;

            Vector2 deltaPos = (mousePos - (Vector2)shootPoint.position).normalized;

            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg - 90f));
            bulletInstance.GetComponent<Bullet>().constructor(self, target, damagePerBullet);
            Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

            rb.velocity = deltaPos * bulletSpeed;
        }
    }

    // Data in scriptableObject is mutable throught play mode, using this little hack to prevent starting fire delay
    public void OnValidate()
    {
        nextFire = 0;
        bullets = bulletsPerCharger;
    }
}