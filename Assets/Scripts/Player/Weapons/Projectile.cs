using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private GameObject shooter;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void SetStats(GameObject go, int dam)
    {
        shooter = go;
        damage = dam;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != shooter && collision.tag != "Projectile" && collision.tag != "Pickup")
        {
            if (shooter.tag == "Player")
            {
                if (collision.tag == "Enemy")
                {
                    Stats s = collision.gameObject.GetComponent<Stats>();
                    s.TakeDamage(damage);
                }
            } else
            {
                if (collision.tag == "Player")
                {
                    //Damage player

                    Stats ps = collision.gameObject.GetComponent<Stats>();
                    ps.TakeDamage(damage);
                }
            }

            rb.velocity = Vector2.zero;

            if (anim)
            {
                anim.SetTrigger("Explode");
                Destroy(gameObject, 0.4f);
            } else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
