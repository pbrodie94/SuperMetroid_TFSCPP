using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float detectionRadius;

    protected float timeLastAttacked;
    protected bool alerted = false;
    protected float distToPlayer;
    protected bool hostile = true;

    protected Transform player;
    protected SamusStatus sms;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

     protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sms = player.gameObject.GetComponent<SamusStatus>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Initialize()
    {

    }

    protected virtual void Update()
    {
        if (player && hostile)
        {
            distToPlayer = Vector3.Distance(transform.position, player.position);
        }
    }

    public void GetPlayerLocation()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void PlayerDeath()
    {
        alerted = false;
        hostile = false;
    }

    public void PlayerSpawned()
    {
        hostile = true;
    }
}
