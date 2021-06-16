using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallZebesian : EnemyAI
{
    [SerializeField] private float crawlSpeed = 3;
    [SerializeField] private float yMinMax;
    [SerializeField] private float minMoveTime = 0.5f;
    private float lastPositionCheck;

    [Header("Combat Preferences")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float projectileForce = 15;
    [SerializeField] private float attackFrequency = 3;
    [SerializeField] private float fireRate = 3;
    
    private float timeLastFired;
    private bool canShoot = true;
    private bool shooting = false;

    [Header("Combat Objects")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePointR;
    [SerializeField] private Transform firePointL;

    [Header("Detection Points")]
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;

    [Header("Audio")]
    [SerializeField] private AudioSource bodyAudio;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip[] moveSound;

    private float moveSoundInterval = 1.5f;
    private float timeLastMoveSound = 0;

    protected override void Start()
    {
        base.Start();

        if (spriteRenderer.flipX)
        {
            Collider2D col = GetComponent<Collider2D>();
            Vector2 colPos = col.offset;
            colPos.x *= -1;
            col.offset = colPos;

            Vector3 checkPos = top.localPosition;
            checkPos.x *= -1;
            top.localPosition = checkPos;

            checkPos = bottom.localPosition;
            checkPos.x *= -1;
            bottom.localPosition = checkPos;
        }

        fireRate = 60 / fireRate;
    }

    public override void Initialize()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 2, LayerMask.GetMask("Wall"));
        Vector3 point = col.ClosestPoint(new Vector2(transform.position.x, transform.position.y));

        //If the wall is to the right, flip the enemy
        if (point.x > transform.position.x)
        {
            //Flip the sprite renderer
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
            //Flip the collider
            Vector2 colPos = col.offset;
            colPos.x *= -1;
            col.offset = colPos;

            //Set the check positions
            Vector3 checkPos = top.localPosition;
            checkPos.x *= -1;
            top.localPosition = checkPos;

            checkPos = bottom.localPosition;
            checkPos.x *= -1;
            bottom.localPosition = checkPos;
        }

        transform.position = point;
    }

    protected override void Update()
    {
        base.Update();

        if (hostile && distToPlayer <= detectionRadius || alerted)
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

            if (Time.time >= timeLastFired + fireRate && !sms.isDead)
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
                if (PathClear(false))
                {
                    rb.velocity = new Vector2(0, -crawlSpeed);

                    anim.SetFloat("Speed", -crawlSpeed);

                    if (bodyAudio && moveSound[0])
                    {
                        int moveIndex = 0;

                        if (moveSound.Length > 1)
                        {
                            moveIndex = Random.Range(0, moveSound.Length - 1);
                        }

                        if (Time.time >= timeLastMoveSound + moveSoundInterval)
                        {
                            bodyAudio.PlayOneShot(moveSound[moveIndex]);

                            timeLastMoveSound = Time.time;
                        }
                    }

                } else
                {
                    anim.SetFloat("Speed", 0);
                    rb.velocity = Vector2.zero;
                    canShoot = true;
                    return;
                }

            }
            else if (player.position.y > transform.position.y && !shooting)
            {
                //Move up
                if (PathClear(true))
                {
                    rb.velocity = new Vector2(0, crawlSpeed);

                    anim.SetFloat("Speed", crawlSpeed);

                    if (bodyAudio && moveSound[0])
                    {
                        int moveIndex = 0;

                        if (moveSound.Length > 1)
                        {
                            moveIndex = Random.Range(0, moveSound.Length - 1);
                        }

                        if (Time.time >= timeLastMoveSound + moveSoundInterval)
                        {
                            bodyAudio.PlayOneShot(moveSound[moveIndex]);

                            timeLastMoveSound = Time.time;
                        }
                    }

                } else
                {
                    anim.SetFloat("Speed", 0);
                    rb.velocity = Vector2.zero;
                    canShoot = true;
                    return;
                }
            }
        }
    }

    bool PathClear(bool up)
    {
        Vector3 check;
        Vector2 dir;

        if (up)
        {
            //Check for top
            check = top.position;
            dir = Vector2.up;
        } else
        {
            //Check for bottom
            check = bottom.position;
            dir = Vector2.down;
        }

        RaycastHit2D hit = Physics2D.Raycast(check, dir, 0.3f);

        if (hit)
        {
            return false;
        } else
        {
            return true;
        }
    }

    void Shoot()
    {
        Transform firePoint;

        if (!spriteRenderer.flipX)
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

        if (bodyAudio && attackSound)
        {
            bodyAudio.PlayOneShot(attackSound);
        }
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