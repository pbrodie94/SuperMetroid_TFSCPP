using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Projectile")
        {
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
