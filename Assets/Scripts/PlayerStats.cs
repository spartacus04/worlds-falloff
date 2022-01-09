using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public int defPoints = 0;
    public GameObject droppedWeapon;
    public SpriteRenderer sp;

    public Slider healthbar;
    public TextMeshProUGUI healthText;
    public RectTransform ammoTransform;
    public TextMeshProUGUI ammoText;
    public GunGeneric[] guns = new GunGeneric[3];
    public Transform shootPoint;
    public float regenRate;
    public float shotTimeOffset;
    public GameObject deadSelf;
    public AudioSource music;
    public AudioClip sfx;
    public bool loreCollected = false;

    public Image[] images;
    public Image[] slots;
    public Animator animator;

    public int selectedInventorySlot = 0;
    private Movement movement;
    private Rigidbody2D rb;
    private bool canShoot = true;
    private float shotTime;

    public void Start()
    {
        movement = GetComponent<Movement>();
        rb = sp.GetComponent<Rigidbody2D>();
        renderMenu();
    }

    void playFadeOut()
    {
        animator.SetTrigger("CanFadeOut");
    }

    void playDeadSound()
    {
        AudioSource.PlayClipAtPoint(sfx, transform.position);
    }

    public void dealDamage(int damage)
    {
        shotTime = Time.time + shotTimeOffset;
        health -= damage - (damage / 100 * defPoints);
        healthbar.value = health;
        healthText.text = $"{health}/{healthbar.maxValue}";
        if (health <= 0)
        {
            healthbar.value = 0;
            healthText.text = $"{0}/{healthbar.maxValue}";
            canShoot = false;
            Instantiate(deadSelf, transform.position, transform.rotation);
            music.Stop();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Movement>().canMove = false;
            
            playDeadSound();
            Invoke("playDeadSound", 1f);
            Invoke("playFadeOut", 3f);
            Invoke("LoadGameOver", 5.5f);
        }
    }

    void LoadGameOver()
    {
        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
    }

    public void increaseArmor(int armorPoints)
    {
        if (defPoints < armorPoints)
        {
            defPoints = armorPoints;
        }
    }

    public GunGeneric getCurrentGun()
    {
        return guns[selectedInventorySlot];
    }

    public void Update()
    {
        if(SceneManager.GetActiveScene().name == "GameOverScene" || SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            Destroy(gameObject);
        }

        // Natural regen
        if(Time.time >= shotTime && canShoot)
        {
            shotTime += regenRate;
            if(rb.velocity == new Vector2(0, 0))
            {
                health += 4;
            }
            health += 1;
            if (health > maxHealth)
                health = maxHealth;
            healthbar.value = health;
            healthText.text = $"{health}/{healthbar.maxValue}";
        }

        // Firing
        if (Input.GetButton("Fire1") && canShoot)
        {
            if (getCurrentGun() != null)
                getCurrentGun().shoot(shootPoint, gameObject, "Enemy", GetComponent<Movement>().mousePos);
        }

        // Selection
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedInventorySlot = 0;
            renderMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedInventorySlot = 1;
            renderMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedInventorySlot = 2;
            renderMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && getCurrentGun() != null)
        {
            // Drop Weapon
            GunGeneric gun = guns[selectedInventorySlot];
            guns[selectedInventorySlot] = null;

            GameObject rwInstance = Instantiate(droppedWeapon, transform.position, Quaternion.identity);

            DroppedWeapon rwScript = rwInstance.GetComponent<DroppedWeapon>();
            rwScript.setGun(gun, Time.time + 5f);

            renderMenu();
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            if (selectedInventorySlot == 0)
            {
                selectedInventorySlot = 2;
            }
            else --selectedInventorySlot;
            renderMenu();
        }
        else if(Input.mouseScrollDelta.y > 0)
        {
            if(selectedInventorySlot == 2)
            {
                selectedInventorySlot = 0;
            }
            else ++selectedInventorySlot;
            renderMenu();
        }
        
        
        
        // Show gun in hand
        if (getCurrentGun() != null)
        {
            sp.sprite = getCurrentGun().weaponInHand;
            sp.color = new Color(255, 255, 255);

            // Render reload time if needed
            if(getCurrentGun().isReloading)
            {
                ammoTransform.sizeDelta = new Vector2(ammoTransform.sizeDelta.x, Mathf.Lerp(64, 0, (getCurrentGun().nextFire - Time.time) / getCurrentGun().reloadTime));
            }

            if(getCurrentGun().bullets == getCurrentGun().bulletsPerCharger)
            {
                ammoText.text = getCurrentGun().bullets.ToString();
            }
        }
        else
        {
            sp.color = new Color(255, 255, 255, 0);
        }
    }

    public void renderMenu()
    {
        // Renders Gun on inventory
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
            {
                images[i].color = new Color(255, 255, 255, 255);
                images[i].sprite = guns[i].artwork;
            }
            else
            {
                images[i].color = new Color(255, 255, 255, 0);
            }

            if (i == selectedInventorySlot)
            {
                slots[i].color = new Color(255, 255, 0);
                continue;
            }
            slots[i].color = new Color(255, 255, 255);
        }

        // Ammo UI
        if (getCurrentGun() == null)
        {
            ammoTransform.parent.gameObject.SetActive(false);
            return;
        }

        ammoTransform.parent.gameObject.SetActive(true);
        ammoText.text = getCurrentGun().bullets.ToString();
        float percent = (float)getCurrentGun().bullets / (float)getCurrentGun().bulletsPerCharger;

        ammoTransform.sizeDelta = new Vector2(ammoTransform.sizeDelta.x, 64 * percent);
    }

    public bool addGun(GunGeneric gun)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] == null)
            {
                guns[i] = gun;
                renderMenu();
                return true;
            }
        }
        return false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DroppedWeapon>() != null)
        {
            DroppedWeapon weapon = collision.gameObject.GetComponent<DroppedWeapon>();
            if (Time.time < weapon.pickupTime) return;

            if (addGun(Instantiate(weapon.gun)))
            {
                Destroy(weapon.gameObject);
            }
        }
    }
}