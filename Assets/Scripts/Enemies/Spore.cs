using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{

    private int damage;

    private Rigidbody2D rb;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Float around
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If spore hits player, deal damage   
        if (collision.gameObject.tag == "Player")
        {
            Stats s = collision.gameObject.GetComponent<Stats>();
            s.TakeDamage(damage);

            Destroy(gameObject);
        }

        //If spore hits anything else, ricochet

    }
}
