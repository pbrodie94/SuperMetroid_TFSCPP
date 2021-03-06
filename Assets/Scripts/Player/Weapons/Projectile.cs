using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
    private int damage;
    [SerializeField] private bool missile = false;
    [SerializeField] private AudioClip impact;
    private GameObject shooter;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            if (!shooter || shooter.tag != "Player")
            {
                if (collision.tag == "Player")
                {
                    //Damage player

                    Stats ps = collision.gameObject.GetComponent<Stats>();
                    ps.TakeDamage(damage);
                }
               
            } else
            {
                if (collision.tag == "Enemy")
                {
                    Stats s = collision.gameObject.GetComponent<Stats>();
                    s.TakeDamage(damage);
                }

                if (collision.tag == "Door")
                {
                    Door d = collision.gameObject.GetComponent<Door>();
                    d.UnlockDoor();

                    if (missile)
                    {
                        d.DestroyMissileLock();
                    }
                }
            }

            rb.velocity = Vector2.zero;

            audioSource.PlayOneShot(impact);

            if (anim)
            {
                anim.SetTrigger("Explode");
                Destroy(gameObject, impact.length);
            } else
            {
                Destroy(gameObject, impact.length);
            }
            
        }
    }
}
