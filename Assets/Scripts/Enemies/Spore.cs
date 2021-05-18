using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private int damage = 23;
    [SerializeField] private float lifeTime = 10;
    private float movementSmoothing = 0.5f;
    private Vector3 moveVelocity = Vector3.zero;
    private float timeSpawned;

    private Transform player;
    private Collider2D col;
    private PickupDropper puDropper;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        puDropper = GetComponent<PickupDropper>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if (damage <= 0)
        {
            damage = 20;
        }

        if (moveSpeed <= 0)
        {
            moveSpeed = 10;
        }

        timeSpawned = Time.time;
    }

    private void FixedUpdate()
    {
        //Float around
        Vector3 dir = player.position - transform.position;
        Vector3 targetVelocity = (dir.normalized * moveSpeed * Time.fixedDeltaTime);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref moveVelocity, movementSmoothing);

        if (Time.time >= timeSpawned + lifeTime)
        {
            rb.velocity = Vector2.zero;
            col.enabled = false;
            anim.SetTrigger("Explode");
            puDropper.DropPickup();

            Destroy(gameObject, 1);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Boss" && collision.tag != "Enemy")
        {
            rb.velocity = Vector2.zero;
            moveSpeed = 0;

            //If spore hits player, deal damage   
            if (collision.gameObject.tag == "Player")
            {
                Stats s = collision.gameObject.GetComponent<Stats>();
                s.TakeDamage(damage);
            }

            //If spore hits anything else destroy
            col.enabled = false;
            anim.SetTrigger("Explode");
            puDropper.DropPickup();

            Destroy(gameObject, 1);
        }
    }
}
