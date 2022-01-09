using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float distanceFromPlayer;
    public float speed;
    public GunGeneric gun;
    public GameObject player;
    public int maxHealth;
    public int health;
    public float rotationOffset;
    public Color deadColor = new Color(87, 67, 67, 255);
    private SpriteRenderer spriteRenderer;
    public bool needDestroy = false;

    public TextMeshProUGUI text;
    public Slider slider;
    public Canvas canvas;
    public Vector2 canvasPos;

    [HideInInspector]
    public bool enemyEnabled;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        canvasPos = (Vector2)canvas.transform.position - (Vector2)gameObject.transform.position;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gun = Instantiate(gun);
        slider.maxValue = maxHealth;

    }
    
    // Update is called once per frame
    void Update()
    {
        canvas.transform.position = (Vector2)gameObject.transform.position + canvasPos;
        canvas.transform.rotation = Quaternion.identity;
        text.text = $"{health}/{maxHealth}";
        slider.value = health;

        if(health <= 0)
        {
            if(needDestroy) Destroy(gameObject);
            GetComponent<Collider2D>().enabled = false;
            Destroy(rb);
            spriteRenderer.color = deadColor;
            if(GetComponent<Animator>() != null)
                GetComponent<Animator>().enabled = false;
            Destroy(slider.gameObject);
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        if(enemyEnabled)
        {
            if (gun != null)
                gun.shoot(gameObject.transform, gameObject, "Player", GetComponent<Enemy>().player.transform.position);

            Vector2 lookDir = (Vector2)player.transform.position - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + rotationOffset;
            rb.rotation = angle;


            if(player.transform.position.x < transform.position.x && rotationOffset == 0)
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }


            if (Vector2.Distance(rb.position, (Vector2)player.transform.position) >= distanceFromPlayer)
            {
                rb.velocity = (Vector2)rb.transform.up * speed * 200 * Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnValidate()
    {
        health = maxHealth;
    }
}
