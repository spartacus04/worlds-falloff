using UnityEngine;

public class RocketProjectile : Bullet
{
    public float radious;
    public AudioClip explosionSound;
    public ParticleSystem particles;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        particles.Play();

        if (self == null) return;

        if (target == "Enemy")
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radious);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].gameObject == gameObject) continue;

                if (hit[i].gameObject.GetComponent<Enemy>() != null)
                    hit[i].gameObject.GetComponent<Enemy>().health -= damage;
                else if (hit[i].gameObject.GetComponent<BomberEnemy>() != null)
                    hit[i].gameObject.GetComponent<BomberEnemy>().health -= damage;
            }
        }
        else
        {
            collision.gameObject.GetComponent<PlayerStats>().dealDamage(damage);
        }

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1f);
    }
}