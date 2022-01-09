using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    public float playerDetectionRadious;
    public float explosionRadious;
    public int explosionDamage;
    public GameObject player;
    public int health;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    public Color explodedColor;
    public AudioClip explosionSound;
    public new ParticleSystem particleSystem;
    private bool exploded = false;

    [HideInInspector]
    public bool enemyEnabled;
    [HideInInspector]
    public Vector2 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            explode(false);
        }
    }

    private void FixedUpdate()
    {
        if (exploded) return;

        if (enemyEnabled)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadious);

            if(containsPlayer(colliders))
            {
                spriteRenderer.sprite = frames[1];
                if (containsPlayer(Physics2D.OverlapCircleAll(transform.position, explosionRadious)))
                {
                    explode(true);
                }
            }
            else
            {
                spriteRenderer.sprite = frames[0];
            }
        }
    }

    public bool containsPlayer(Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == player) return true;
        }
        return false;
    }

    public void explode(bool checkForPlayer)
    {

        exploded = true;
        spriteRenderer.sprite = frames[2];
        spriteRenderer.color = explodedColor;

        GetComponent<Collider2D>().enabled = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkForPlayer ? explosionRadious : explosionRadious * 2);

        particleSystem.Play();
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.CompareTag("Player") && checkForPlayer)
            {
                player.GetComponent<PlayerStats>().dealDamage(explosionDamage);
            }

            if(colliders[i].gameObject.CompareTag("Enemy") && colliders[i].gameObject != gameObject)
            {
                if (colliders[i].gameObject.GetComponent<Enemy>() != null)
                    colliders[i].gameObject.GetComponent<Enemy>().health -= explosionDamage;
                else
                    colliders[i].gameObject.GetComponent<BomberEnemy>().health -= explosionDamage;
            }

            if(colliders[i].gameObject.CompareTag("BossC"))
            {
                GameObject.FindGameObjectsWithTag("Boss")[0].GetComponent<FinalBoss>().damage();
            }
        }

        Destroy(this);
    }
}
