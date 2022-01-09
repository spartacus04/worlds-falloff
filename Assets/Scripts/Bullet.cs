using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public string target;
    public GameObject self;
    public bool instantiated;

    // Using this little hack to construct object
    public void constructor(GameObject self, string target, int damage)
    {
        Physics2D.IgnoreCollision(self.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        this.target = target;
        this.self = self;
        this.damage = damage;
        instantiated = true;
    }

    public void Update()
    {
        if(instantiated && self == null) Destroy(gameObject);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (self == null) return;
        if(collision.gameObject.CompareTag(target))
        {
            if(target == "Enemy")
            {
                if(collision.gameObject.GetComponent<Enemy>() != null)
                    collision.gameObject.GetComponent<Enemy>().health -= damage;
                else
                    collision.gameObject.GetComponent<BomberEnemy>().health -= damage;
            }
            else
            {
                collision.gameObject.GetComponent<PlayerStats>().dealDamage(damage);
            }
        }

        if(collision.gameObject != self)
        {
            Destroy(gameObject);
            //TODO: destroy particle particle
        }
    }
}
