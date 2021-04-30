using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallZebesian : MonoBehaviour
{
    [SerializeField] private float crawlSpeed = 3;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float yMinMax;
    [SerializeField] private float minMoveTime = 1;
    private float lastPositionCheck;

    [Header("Combat Preferences")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float projectileForce = 15;
    [SerializeField] private float attackFrequency = 3;
    [SerializeField] private float fireRate = 3;
    private float timeLastAttacked;
    private float timeLastFired;
    private bool canShoot = true;
    private bool shooting = false;

    [Header("Combat Objects")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePointR;
    [SerializeField] private Transform firePointL;

    private bool alerted = false;
    private float distToPlayer;

    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer zebesian;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        zebesian = GetComponent<SpriteRenderer>();

        if (zebesian.flipX)
        {
            Collider2D col = GetComponent<Collider2D>();
            Vector2 colPos = col.offset;
            colPos.x *= -1;
            col.offset = colPos;
        }

        fireRate = 60 / fireRate;
    }

    private void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= detectionRadius || alerted)
        {
            alerted = true;
            anim.SetBool("Alerted", true);
        } else
        {
            anim.SetBool("Alerted", false);
        }

        if (alerted)
        {
            Alerted();
        }
    }

    void Alerted()
    {
        AdjustPosition();

        if (canShoot && Time.time >= timeLastAttacked + attackFrequency)
        {
            anim.SetTrigger("Attack");
        }

        if (shooting)
        {
            rb.velocity = Vector2.zero;

            if (Time.time >= timeLastFired + fireRate)
            {
                timeLastFired = Time.time;

                Shoot();
            }
        }
    }

    void AdjustPosition()
    {
        //Add code to check for end of wall, and ground

        bool inRange = true;

        if (Time.time >= lastPositionCheck + minMoveTime)
        {
            lastPositionCheck = Time.time;

            if (transform.position.y <= player.position.y + yMinMax && transform.position.y >= player.position.y - yMinMax)
            {
                inRange = true;

                anim.SetFloat("Speed", 0);
                canShoot = true;
                rb.velocity = Vector2.zero;

            }
            else
            {
                inRange = false;
            }
        }

        if (!inRange)
        {
            canShoot = false;

            if (player.position.y < transform.position.y && !shooting)
            {
                //Move down
                rb.velocity = new Vector2(0, -crawlSpeed);

                anim.SetFloat("Speed", -crawlSpeed);

            }
            else if (player.position.y > transform.position.y && !shooting)
            {
                //Move up
                rb.velocity = new Vector2(0, crawlSpeed);

                anim.SetFloat("Speed", crawlSpeed);
            }
        }
    }

    void Shoot()
    {
        Transform firePoint;

        if (!zebesian.flipX)
        {
            firePoint = firePointR;
        } else
        {
            firePoint = firePointL;
        }


        Vector3 targetPos = player.position;
        targetPos.y += 1;
        Vector3 shootDir = targetPos - firePoint.position;
        shootDir.Normalize();
        shootDir.z = 0;

        GameObject go = Instantiate(projectile, firePoint.position, Quaternion.identity);
        Rigidbody2D body = go.GetComponent<Rigidbody2D>();
        Projectile proj = go.GetComponent<Projectile>();
        proj.SetStats(gameObject, damage);
        body.AddForce(shootDir * projectileForce);
    }

    public void BeginAttack()
    {
        shooting = true;
    }

    public void EndAttack()
    {
        timeLastAttacked = Time.time;
        shooting = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}